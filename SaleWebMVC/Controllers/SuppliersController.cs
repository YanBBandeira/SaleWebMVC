using Microsoft.AspNetCore.Mvc;
using SalesWebMVC.Models;
using SalesWebMVC.Models.ViewModels;
using SalesWebMVC.Services;

namespace SalesWebMVC.Controllers
{
    public class SuppliersController : Controller
    {
        private readonly SuppliersService _supplierService;
        private readonly ProductsService _productsService;
        private readonly LocationService _locationService;

        public SuppliersController(SuppliersService supplierService, ProductsService productsService, LocationService locationService)
        {
            _supplierService = supplierService;
            _productsService = productsService;
            _locationService = locationService;
        }
        public async Task<IActionResult> Index()
        {
            var list = await _supplierService.FindAllAsync();
            return View(list);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var states = await _locationService.FindAllStatesAsync();
            var cities = await _locationService.FindAllCitiesAsync();
            var viewModel = new SupplierFormViewModel
            {
                Supplier = new Supplier(),
                States = states,
                Cities = cities
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SupplierFormViewModel viewModel)
        {
            await _supplierService.InsertAsync(viewModel.Supplier);
            return RedirectToAction(nameof(Index));
            //if (ModelState.IsValid)
            //{
            //    await _supplierService.InsertAsync(viewModel.Supplier);
            //    return RedirectToAction(nameof(Index));
            //}
            ////se falhar, recarrega a lista de estados para o form
            //viewModel.States = await _locationService.FindAllStatesAsync();
            //viewModel.Cities = await _locationService.FindAllCitiesAsync();
            //return View(viewModel);
        }

        



    }
}
