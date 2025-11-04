using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using SalesWebMVC.Data;
using SalesWebMVC.Models;
using SalesWebMVC.Models.Enums;
using SalesWebMVC.Services.Exceptions;
using System.Linq;
using System.Runtime.CompilerServices;

namespace SalesWebMVC.Services
{
    public class SalesRecordService
    {
        private readonly ApplicationDbContext _context;
        private readonly InventoryService _inventoryService;

        public SalesRecordService(ApplicationDbContext context, InventoryService inventoryService)
        {
            _context = context;
            _inventoryService = inventoryService;
        }

        public async Task InsertAsync(SalesRecord obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(nameof(obj), "The SalesRecord object cannot be null");
            }
            if (obj.OrderItems == null || !obj.OrderItems.Any())
            {
                throw new ArgumentException("The SalesRecord must have at least one OrderItem.");
            }

            try
            {
                // Regra de Negócio: Garante que vendas recém-criadas são Pendentes.
                obj.Status = SalesStatus.Pending;
                double calculatedTotal = 0.0;

                // 2. Lógica de Segurança: Recalcula o Total com Preços do BD
                foreach (var item in obj.OrderItems)
                {
                    // Busca o preço real do produto no banco de dados
                    var product = await _context.Products.FindAsync(item.ProductId);

                    if (product == null)
                    {
                        // Se o produto não existir, a venda é inválida.
                        throw new IntegrityException($"Produto com ID {item.ProductId} não encontrado.");
                    }

                    // Atribui o preço REAL do banco de dados (Ignora o valor do frontend!)
                    item.UnitPrice = product.Price;

                    // Recalcula o subtotal para a soma
                    calculatedTotal += item.Quantity * item.UnitPrice;
                }

                // 3. Atribui o Total Recalculado (Total seguro)
                obj.Amount = calculatedTotal;

                // 4. Adiciona a entidade pai (o EF Core deve rastrear os OrderItems)
                _context.OrderItems.AddRange(obj.OrderItems);
                _context.SalesRecords.Add(obj); // Nome da DbSet corrigido para SalesRecords

                // 5. Salva todas as alterações no banco de dados
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw new IntegrityException("Fail at saving sale.");
            }
        }

        public async Task RemoveAsync(int id)
        {
            try
            {
                var obj = await _context.SalesRecords.FindAsync(id);
                if (obj == null)
                {
                    throw new NotFoundException("SalesRecord not found");
                }
                _context.Remove(obj);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                throw new IntegrityException(ex.Message);
            }
        }

        public async Task UpdateAsync(SalesRecord obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(nameof(obj), "The SalesRecord object cannot be null");
            }
            bool hasAny = await _context.SalesRecords.AnyAsync(x => x.Id == obj.Id);
            if (!hasAny)
            {
                throw new NotFoundException("Id not found");
            }
            try
            {
                _context.Update(obj);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new DbConcurrencyException(ex.Message);
            }
        }

        // NOVO MÉTODO DE FILTRAGEM EFICIENTE (CARD-05)
        public async Task<List<SalesRecord>> FindAllByFiltersAsync(
            string sellerName,
            List<int> departmentIds,
            DateTime? minDate,
            DateTime? maxDate,
            List<SalesStatus> statusIds)
        {
            // 1. Query inicial: Traz todos os registros de vendas
            var result = from obj in _context.SalesRecords select obj;

            // 2. Eager Loading (Inclusão das dependências)
            result = result
                .Include(x => x.Seller)
                .Include(x => x.Seller.Department)
                .Include(x => x.OrderItems) // Inclui os itens de venda, se necessário para a tabela
                .OrderByDescending(x => x.Date); // Ordena pela data mais recente

            // 3. Aplicar Filtros Condicionais

            // Filtro por Data Mínima
            if (minDate.HasValue)
            {
                result = result.Where(x => x.Date >= minDate.Value.Date);
            }

            // Filtro por Data Máxima (Garante que inclui o dia inteiro)
            if (maxDate.HasValue)
            {
                // Ajusta para incluir até o final do dia
                var maxDateTime = maxDate.Value.Date.AddDays(1).AddSeconds(-1);
                result = result.Where(x => x.Date <= maxDateTime);
            }

            // Filtro por Nome do Vendedor (Partial Match Case Insensitive)
            if (!string.IsNullOrWhiteSpace(sellerName))
            {
                // Usa Contains para pesquisa parcial, o que se traduz em um LIKE no SQL.
                // O banco de dados pode gerenciar a insensibilidade de caso dependendo da configuração.
                result = result.Where(x => x.Seller.UserFullName.Contains(sellerName));
            }

            // Filtro por Departamentos (Lista de IDs)
            if (departmentIds != null && departmentIds.Any())
            {
                // Filtra se o DepartmentId do vendedor estiver na lista de IDs fornecida
                result = result.Where(x => departmentIds.Contains(x.Seller.DepartmentId.Value));
            }

            // Filtro por Status de Venda (Lista de Enums)
            if (statusIds != null && statusIds.Any())
            {
                // Filtra se o Status da venda estiver na lista de Status fornecida
                result = result.Where(x => statusIds.Contains(x.Status));
            }

            // 4. Executa a query no banco de dados
            return await result.ToListAsync();
        }
        public async Task<List<SalesRecord>> FindAllByAsync()
        {
            return await _context.SalesRecords
                .Include(x => x.Seller)
                .ThenInclude(x => x.Department).ToListAsync();
        }

        public async Task<SalesRecord> FindByIdAsync(int id)
        {
            return await _context.SalesRecords
                 .Include(x => x.Seller)
                 .ThenInclude(x => x.Department)
                 .FirstOrDefaultAsync(obj => obj.Id == id);
        }


        // ========================================================================= 
        // MÉTODO DE NEGÓCIO PRINCIPAL: FATURAMENTO E BAIXA DE ESTOQUE 
        // ========================================================================= 
        public async Task BillSaleAndDecrementStockAsync(int saleId, string userId)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {   // 2. BUSCAR A VENDA COM OS OrderItems
                var sale = await _context.SalesRecords
                    .Include(sr => sr.OrderItems)
                    .ThenInclude(oi => oi.Product)
                    .FirstOrDefaultAsync(sr => sr.Id == saleId);
                if (sale == null)
                {
                    throw new NotFoundException($"SalesRecord Id {saleId} not found");
                }
                if (sale.Status != Models.Enums.SalesStatus.Pending)
                {
                    throw new InvalidOperationException("Only pending sales can be billed.");
                }
                if (sale.OrderItems == null || !sale.OrderItems.Any())
                {
                    throw new InvalidOperationException("Sale has no associated order items.");
                }
                // 3. PROCESSAMENTO DO ESTOQUE
                foreach (var item in sale.OrderItems)
                {
                    var inventoryMovement = new InventoryMovement
                    {
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        MovementType = Models.Enums.MovementType.Exit,
                        Date = DateTime.Now,
                        UserId = userId,
                        Observation = $"Sale billed - Sale ID: {sale.Id}, Product ID: #{item.ProductId}, Product Name: {item.Product.Name}"
                    };
                    // Chama o InventoryService para checar estoque, registrar movimento e atualizar produto 
                    await _inventoryService.CreateMovementAsync(inventoryMovement);
                }
                // 4. ATUALIZAR STATUS DA VENDA
                sale.Status = Models.Enums.SalesStatus.Billed;
                _context.SalesRecords.Update(sale);
                await _context.SaveChangesAsync();
                // 5. COMMIT: Confirma todas as alterações no banco de dados 
                await transaction.CommitAsync();

            }
            // 6. TRATAMENTO DE EXCEÇÃO (ROLLBACK - CARD-04) 
            catch (InvalidOperationException ex) when (ex.Message.Contains("Insufficient stock for removal"))
            {
                // Rollback para desfazer todos os movimentos de estoque e a atualização do status da  venda
                await transaction.RollbackAsync();
                throw new IntegrityException($"Faturamento falhou devido a estoque insuficiente: {ex.Message}");
            }
            catch (Exception)
            {
                // Rollback para qualquer outra falha (DB, etc.) 
                await transaction.RollbackAsync();
                throw;
            }
        }

        // Dentro da classe SalesRecordService

        /// <summary>
        /// Cancela uma venda faturada, estornando (incrementando) o estoque de todos os OrderItems
        /// e gerencia a transação para garantir atomicidade.
        /// </summary>
        /// <param name="saleId">ID do SalesRecord a ser cancelado.</param>
        /// <param name="userId">ID do usuário que está cancelando a venda.</param>
        /// <exception cref="NotFoundException">Se a venda não for encontrada.</exception>
        /// <exception cref="IntegrityException">Se a venda não estiver em status Billed ou se houver falha de DB/estoque.</exception>
        public async Task CancelSaleAndRestockAsync(int saleId, string userId)
        {
            // 1. INÍCIO DA TRANSAÇÃO EXPLÍCITA
            await using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // 2. BUSCAR A VENDA COM OS OrderItems
                var sale = await _context.SalesRecords
                    .Include(sr => sr.OrderItems)
                    .ThenInclude(oi => oi.Product)
                    .FirstOrDefaultAsync(sr => sr.Id == saleId);

                if (sale == null)
                {
                    throw new NotFoundException($"Sale ID {saleId} not found.");
                }

                // 2a. VALIDAÇÃO DE STATUS: Apenas vendas faturadas (Billed) podem ser canceladas e estornadas.
                if (sale.Status != SalesStatus.Billed)
                {
                    throw new IntegrityException($"Only billed sales can be canceled and restocked. Current status: {sale.Status}.");
                }

                // 3. PROCESSAMENTO DO ESTORNO DE ESTOQUE (Incremento)
                foreach (var item in sale.OrderItems)
                {
                    if (item.Quantity <= 0) continue;

                    var movement = new InventoryMovement
                    {
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        MovementType = MovementType.Devolution, // Usa Devolução ou Entrada
                        Date = DateTime.Now,
                        UserId = userId,
                        Observation = $"Sale canceled Sale-ID: #{saleId}, Product-ID: #{item.ProductId}, Product Name: {item.Product.Name}"
                    };

                    // O InventoryService fará o SaveChanges, registrando a entrada e incrementando o Product.StockQuantity
                    await _inventoryService.CreateMovementAsync(movement);
                }

                // 4. ATUALIZAR STATUS DA VENDA
                sale.Status = SalesStatus.Canceled;
                _context.SalesRecords.Update(sale);
                await _context.SaveChangesAsync();

                // 5. COMMIT: Confirma todas as alterações no banco de dados
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                // 6. ROLLBACK: Em caso de qualquer falha (incluindo falha de DB durante o estorno)
                await transaction.RollbackAsync();
                // Relança uma IntegrityException genérica ou a exceção original
                throw new IntegrityException($"Cancellation failed due to an error: {ex.Message}");
            }
        }
    }
}
