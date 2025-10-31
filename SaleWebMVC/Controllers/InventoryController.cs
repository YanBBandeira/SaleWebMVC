using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using SalesWebMVC.Models;

namespace SalesWebMVC.Controllers
{
    [Authorize(Roles = "Admin,Manager")]
    public class InventoryController : Controller
    {
        private readonly Services.InventoryService _inventoryService;
        private readonly Services.ProductsService _productService;
        private readonly Services.SuppliersService _supplierService;
        private readonly UserManager<ApplicationUser> _userManager;

        public InventoryController(Services.InventoryService inventoryService,
            Services.ProductsService productService,
            Services.SuppliersService supplierService,
            UserManager<ApplicationUser> userManager)
        {
            _inventoryService = inventoryService;
            _productService = productService;
            _supplierService = supplierService;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            return View();
        }

        public async Task<IActionResult> GetStockSummaryTable()
        {
            var summary = await _inventoryService.GetStockSummaryAsync();
            return PartialView("_StockSummaryTable", summary);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var products = await _productService.FindAllAsync() ?? new List<Product>();
            ViewBag.Suppliers = await _supplierService.FindAllAsync();
            ViewBag.Products = products;
            //ViewData["ProductId"] = new SelectList(products, "id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(InventoryMovement movement)
        {
            ModelState.Remove(nameof(movement.UserId));
            ModelState.Remove(nameof(movement.User));
            ModelState.Remove(nameof(movement.Product));
            if (!ModelState.IsValid)
            {
                var products = await _productService.FindAllWithSupplirLocationAsync();
                ViewBag.Suppliers = await _supplierService.FindAllAsync();
                ViewBag.Products = await _productService.FindAllAsync() ?? new List<Product>();
                return View(movement);
            }
            try
            {
                var user = await _userManager.GetUserAsync(User);
                movement.UserId = user.Id;

                await _inventoryService.CreateMovementAsync(movement);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                var products = await _productService.FindAllWithSupplirLocationAsync();
                ViewBag.Suppliers = await _supplierService.FindAllAsync();
                ViewBag.Products = await _productService.FindAllAsync() ?? new List<Product>();
                return View(movement);
            }           
        }

        public async Task<IActionResult> Movements()
        {
            return View();
        }

        public async Task<IActionResult> GetMovementHistoryTable()
        {
            var history = await _inventoryService.GetMovementHistoryAsync();
            return PartialView("_MovementsHistoryTable", history);
        }

        public async Task<IActionResult> RestockAlert()
        {
            var lowStockProducts = await _inventoryService.GetRestockAlertsAsync();
            return View(lowStockProducts);
        }
    }
}
