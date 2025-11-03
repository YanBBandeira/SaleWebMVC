using Microsoft.EntityFrameworkCore;
using SalesWebMVC.Data;
using SalesWebMVC.Models;
using SalesWebMVC.Models.Enums;

namespace SalesWebMVC.Services
{
    public class InventoryService
    {
        private readonly ApplicationDbContext _context;
        public InventoryService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Registrar e atualizar o estoque
        public async Task CreateMovementAsync(InventoryMovement movement)
        {
            var product = await _context.Products.FindAsync(movement.ProductId);
            if (product == null)
            {
                throw new Exception("Product not found");
            }
            //Validação de saída (Bloqueiar estoque negativo)
            if (movement.MovementType == MovementType.Exit)
            {
                if (product.StockQuantity < movement.Quantity)
                {
                    throw new Exception("Insufficient stock for removal");
                }
            }

            movement.Date = DateTime.Now; // Garantir que a data seja a atual
            _context.InventoryMovements.Add(movement);

            // Atualizar a quantidade em estoque
            UpdateProductStock(product, movement);
            await _context.SaveChangesAsync();
        }

        private void UpdateProductStock(Product product, InventoryMovement movement)
        {
            if (movement.MovementType == MovementType.Entry || movement.MovementType == MovementType.Devolution)
            {
                product.StockQuantity += movement.Quantity;
            }
            else if (movement.MovementType == MovementType.Exit)
            {
                product.StockQuantity -= movement.Quantity;
            }
            _context.Products.Update(product);
        }


        // Consultas para o controller

        // Método para o Index do InventoryController
        public async Task<ICollection<InventoryMovement>> GetStockSummaryAsync(int? departmentId = null)
        {
            var query = _context.InventoryMovements
                .Include(p => p.User)
                .Include(p => p.Product)
                .Include(p => p.Product.Department)
                .Include(p => p.Product.Supplier)
                .OrderBy(p => p.Product.Name)
                .AsQueryable();
            if (departmentId.HasValue)
            {
                query = query.Where(p => p.Product.DepartmentId == departmentId.Value);
            }

            return await query.ToListAsync();
        }

        public bool ValidateMinimumStock(Product product)
        {
            return product.StockQuantity <= product.MinimumStock;
        }

        public async Task<ICollection<InventoryMovement>> GetMovementHistoryAsync()
        {
            var query = _context.InventoryMovements
                .Include(m => m.Product)
                .ThenInclude(p => p.Supplier)
                .Include(m => m.User)
                .OrderByDescending(m => m.Date)
                .AsNoTracking();
            return await query.ToListAsync();
        }
        
        // Consultas para o relatório 
        public async Task<ICollection<Product>> GetRestockAlertsAsync()
        {
            var query = _context.Products
                .Include(p => p.Department)
                .Include(p => p.Supplier)
                .Where(p => p.StockQuantity <= p.MinimumStock)
                .OrderBy(p => p.Name);
            return await query.ToListAsync();
        }
    }
}

