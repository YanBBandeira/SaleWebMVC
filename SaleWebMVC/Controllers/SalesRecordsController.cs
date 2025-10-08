using Microsoft.AspNetCore.Mvc;
using SalesWebMVC.Models;
using SalesWebMVC.Models.ViewModels;
using SalesWebMVC.Services;

namespace SalesWebMVC.Controllers
{
    public class SalesRecordsController : Controller
    {
        private readonly SalesRecordService _salesRecordService;
        private readonly SellerService _sellerService;
        private readonly DepartmentService _departmentService;

        public SalesRecordsController(SalesRecordService salesRecordService, SellerService sellerService, DepartmentService departmentService)
        {
            _salesRecordService = salesRecordService;
            _sellerService = sellerService;
            _departmentService = departmentService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> SimpleSearch(DateTime? minDate, DateTime? maxDate)
        {
            if (!minDate.HasValue)
            {
                minDate = new DateTime(DateTime.Now.Year,1,1);
            }
            if (!maxDate.HasValue)
            {
                maxDate = DateTime.Now;
            }

            ViewData["minDate"] = minDate.Value.ToString("yyyy-MM-dd");
            ViewData["maxDate"] = maxDate.Value.ToString("yyyy-MM-dd");



            var result = await _salesRecordService.FindByDateAsync(minDate, maxDate);
            return PartialView("_SalesRecordsTable",result);
        }

        public async Task<IActionResult> GroupSearch(DateTime? minDate, DateTime? maxDate)
        {
            if (!minDate.HasValue)
            {
                minDate = new DateTime(DateTime.Now.Year, 1, 1);
            }
            if (!maxDate.HasValue)
            {
                maxDate = DateTime.Now;
            }

            ViewData["minDate"] = minDate.Value.ToString("yyyy-MM-dd");
            ViewData["maxDate"] = maxDate.Value.ToString("yyyy-MM-dd");

            var result = await _salesRecordService.FindByDateGroupingAsync(minDate, maxDate);
            return PartialView("_SalesGroupRecordsTable",result);
        }

        public async Task<IActionResult> Create()
        {
            var sales = await _salesRecordService.FindAllByAsync();
            var sellers = await _sellerService.FindAllAsync();
            var departments = await _departmentService.FindAllAsync();
            var viewModel = new SalesFromViewModel { Sales = sales, Sellers = sellers, Departments = departments };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SalesRecord salesRecord)
        {
            await _salesRecordService.InsertAsync(salesRecord);
            return RedirectToAction(nameof(Index));
        }
    }
}
