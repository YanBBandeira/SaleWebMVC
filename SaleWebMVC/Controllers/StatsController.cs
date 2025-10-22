using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SalesWebMVC.Models.ViewModels;
using SalesWebMVC.Services;
using System.Globalization;

namespace SalesWebMVC.Controllers
{
    public class StatsController : Controller
    {
        private readonly SalesRecordService _salesRecordService;
        private readonly SellerService _sellerService;
        private readonly DepartmentService _departmentService;
        private readonly StatsService _statsService;

        public StatsController(
            SalesRecordService salesRecordService, 
            SellerService sellerService, 
            DepartmentService departmentService,
            StatsService statsService)
        {
            _salesRecordService = salesRecordService;
            _sellerService = sellerService;
            _departmentService = departmentService;
            _statsService = statsService;
        }
        public async Task<IActionResult> Index()
        {
            var data = await _statsService.GetSalesByMonthAsync();
            return View(data);
        }
    }
}
