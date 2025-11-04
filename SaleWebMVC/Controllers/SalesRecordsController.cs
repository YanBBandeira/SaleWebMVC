using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SalesWebMVC.Models;
using SalesWebMVC.Models.Enums;
using SalesWebMVC.Models.ViewModels;
using SalesWebMVC.Services;
using SalesWebMVC.Services.Exceptions;
using System.Security.Claims;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SalesWebMVC.Controllers
{
    public class SalesRecordsController : Controller
    {
        private readonly SalesRecordService _salesRecordService;
        private readonly SellerService _sellerService;
        private readonly DepartmentService _departmentService;
        // NOVO: Adicione ProductService ou InventoryService (dependendo de onde você buscará a lista de produtos)
        private readonly ProductsService _productService;

        public SalesRecordsController(SalesRecordService salesRecordService, SellerService sellerService, DepartmentService departmentService, ProductsService productService)
        {
            _salesRecordService = salesRecordService;
            _sellerService = sellerService;
            _departmentService = departmentService;
            _productService = productService; // Adicionado
        }

        // --- Action Index ---
        public async Task<IActionResult> Index()
        {
            // O Index agora carrega apenas os dados necessários para os filtros e uma lista inicial
            // Se você quiser que a tabela venha vazia, pode passar new List<SalesRecord>()
            var sales = await _salesRecordService.FindAllByAsync();

            ViewBag.Departments = await _departmentService.FindAllAsync();

            // Note que o ViewBag.Sellers foi removido, pois o Seller é carregado dinamicamente via AJAX no Create.
            // Para o filtro na Index, você pode mantê-lo ou carregar os vendedores necessários aqui.
            return View(sales);
        }

        // ----------------------------------------------------------------------------------
        // REVISÃO CRÍTICA: ACTION FILTER (CARD-05) - Filtro na View de Índice
        // ----------------------------------------------------------------------------------
        [HttpGet]
        public async Task<IActionResult> Filter(
            string Seller,
            List<int> DepartmentIds,
            DateTime? minDate,
            DateTime? maxDate,
            List<SalesStatus> SalesStatusIds)
        {
            // A LÓGICA DE FILTRAGEM DEVE ESTAR NO SERVICE, otimizando a consulta ao DB (LINQ to Entities).
            // A Action deve apenas chamar um método de Service que aplica todos os filtros.

            // Você pode criar um DTO ou um método de Service para lidar com todos esses filtros de uma vez.
            // Ex: FindByFiltersAsync(FilterModel filters)

            // Por enquanto, vou supor que o método FindAllByAsync foi refatorado para aceitar esses filtros:
            var sales = await _salesRecordService.FindAllByFiltersAsync(
                Seller,
                DepartmentIds,
                minDate,
                maxDate,
                SalesStatusIds);

            // A View de Índice espera que o filtro retorne a Partial View
            // A Partial View _SalesTablePartial aceita uma lista de SalesRecord.
            return PartialView("_salesTable", sales);
        }

        // --- Action Create (GET) ---
        public async Task<IActionResult> Create()
        {
            var sellers = await _sellerService.FindAllAsync();
            var departments = await _departmentService.FindAllAsync();
            var products = await _productService.FindAllAsync(); // Buscando produtos para o dropdown de itens de venda
            var productsDtos = products.Select(p => new SalesFromViewModel.ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price
            }).ToList();

            var viewModel = new SalesFromViewModel
            {
                SalesRecord = new SalesRecord(),
                Sellers = sellers,
                Departments = departments,
                OrderItems = new List<OrderItem> { new OrderItem() }, // Inicializa com 1 item vazio
                Products = productsDtos
            };

            return View(viewModel);
        }

        // ----------------------------------------------------------------------------------
        // REVISÃO CRÍTICA: ACTION CREATE (POST) - Suporte a OrderItems
        // ----------------------------------------------------------------------------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        // O Model agora é SalesFromViewModel, pois ele contém a lista OrderItems que o View espera
        public async Task<IActionResult> Create(SalesFromViewModel viewModel)
        {
            //Validação dos dados do cabeçalho da venda e dos itens
            //if (!ModelState.IsValid)
            //{
            //    // Se a validação falhar, você deve recarregar os dados para o ViewModel
            //    viewModel.Sellers = await _sellerService.FindAllAsync();
            //    viewModel.Departments = await _departmentService.FindAllAsync();
            //    var products = await _productService.FindAllAsync();
            //    var productsDtos = products.Select(p => new SalesFromViewModel.ProductDto
            //    {
            //        Id = p.Id,
            //        Name = p.Name,
            //        Price = p.Price
            //    }).ToList();
            //    viewModel.Products = productsDtos;
            //    ViewBag.AllProducts = products;
            //    return View(viewModel);
            //}

            try
            {
                // Passa o objeto SalesRecord (que agora contém a lista OrderItems preenchida pelo Model Binder)
                await _salesRecordService.InsertAsync(viewModel.SalesRecord);

                // Retorna um JSON para o AJAX, conforme a sua View Create.cshtml (POST com AJAX)
                return Json(new { success = true, message = "Sales registered with sucess!" });
            }
            catch (IntegrityException ex)
            {
                // Erros de integridade (ex: venda sem itens) devem ser retornados via AJAX
                return BadRequest(new { success = false, message = ex.Message });
            }
            catch (Exception ex)
            {
                // Erro genérico
                return StatusCode(500, new { success = false, message = $"Erro inesperado: {ex.Message}" });
            }
        }


        // --- Actions Bill e Cancel (Mantidas - Elas estão perfeitas para o AJAX) ---
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Bill(int id)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized();
                }
                await _salesRecordService.BillSaleAndDecrementStockAsync(id, userId);
                return Json(new { success = true, message = $"Sales #{id} billed with sucess!" });
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
            catch (IntegrityException ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { success = false, message = "Erro interno ao processar a fatura." });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cancel(int id)
        {
            // ... [Lógica de Cancel (Cancelar) mantida] ...
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized();
                }
                await _salesRecordService.CancelSaleAndRestockAsync(id, userId);
                return Json(new { success = true, message = $"Venda {id} cancelada e estoque estornado com sucesso." });
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
            catch (IntegrityException ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { success = false, message = "Erro interno ao processar o cancelamento." });
            }
        }

        // --- Actions Não Utilizadas/Redundantes (Removidas/Ajustadas) ---
        // SimpleSearch e GroupSearch parecem ser redundantes, pois a Action Filter já cobre todos os campos
        // Você pode removê-las se não forem mais usadas.

        // Details e Edit mantidas, mas precisariam de mais atenção para suportar OrderItems se o foco fosse completo.

        public async Task<IActionResult> Details(int id)
        {
            // O FindByIdAsync DEVE ser atualizado no Service para incluir os OrderItems.
            // ... [Lógica mantida] ...
            if (id == null) return RedirectToAction(nameof(Error), new { message = "Id not provided." });

            var salesRecord = await _salesRecordService.FindByIdAsync(id);
            if (salesRecord == null) return NotFound();

            // Se você quer exibir detalhes do item de venda, o FindByIdAsync deve carregar a lista OrderItems.
            var viewModel = new SalesFromViewModel
            {
                SalesRecord = salesRecord,
                // ... outros dados se necessário (Departments, Sellers)
            };
            return PartialView("_details", viewModel);
        }

        // ... [Outras Actions como Edit, Delete e Error mantidas para brevidade] ...
    }
}