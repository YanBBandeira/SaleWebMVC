using Microsoft.AspNetCore.Mvc;
using SalesWebMVC.Models;
using SalesWebMVC.Models.ViewModels;
using SalesWebMVC.Services;
using System.Threading.Tasks;

namespace SalesWebMVC.Controllers
{
    public class ProductsController : Controller
    {
        private readonly SalesRecordService _salesRecordService;
        private readonly SellerService _sellerService;
        private readonly DepartmentService _departmentService;
        private readonly ProductsService _productsService;
        private readonly SuppliersService _supplierService;
        public ProductsController(SalesRecordService salesRecordService, SellerService sellerService, DepartmentService departmentService, ProductsService productsService, SuppliersService supplierService)
        {
            _salesRecordService = salesRecordService;
            _sellerService = sellerService;
            _departmentService = departmentService;
            _productsService = productsService;
            _supplierService = supplierService;
        }
        public async Task<IActionResult> Index()
        {
            var products = await _productsService.FindAllWithSupplirLocationAsync();
            return View(products);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var suppliers = await _supplierService.FindAllAsync();
            var departments = await _departmentService.FindAllAsync();
            var viewModel = new ProductFormViewModel
            {
                Product = new Product(),
                Suppliers = suppliers,
                Departments = departments
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductFormViewModel viewModel)
        {
            await _productsService.AddProductAsync(viewModel.Product);
            return RedirectToAction(nameof(Index));
        }
        //    if (ModelState.IsValid)
        //    {
        //        await _productsService.AddProductAsync(viewModel.Product);
        //        return RedirectToAction(nameof(Index));
        //    }

        //    //se falhar, recarrega a lista de fornecedores e departamentos para o form
        //    viewModel.Suppliers = await _supplierService.FindAllAsync();
        //    viewModel.Departments = await _departmentService.FindAllAsync();
        //    return View(viewModel);
        //}
    }
}
