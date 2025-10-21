using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SalesWebMVC.Models;
using SalesWebMVC.Models.Enums;
using SalesWebMVC.Models.ViewModels;
using SalesWebMVC.Services;
using static System.Runtime.InteropServices.JavaScript.JSType;

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

        public async Task<IActionResult> Index()
        {
            //Pega todos os registros de vendas e retorna como lista
            var sales = await _salesRecordService.FindAllByAsync();

            // Preparar os dados para manter os filtros na view
            ViewBag.Departments = await _departmentService.FindAllAsync();
            ViewBag.Sellers = await _sellerService.FindAllAsync();
            return View(sales);

        }

        public async Task<IActionResult> Filter(
                 string Seller,
                List<int> DepartmentIds,
                DateTime? minDate,
                DateTime? maxDate,
                List<SalesStatus> SalesStatusIds)
        {
            //Pega todos os registros de vendas e retorna como lista
            var sales = await _salesRecordService.FindAllByAsync();

            // Filtro por vendedor
            if (!string.IsNullOrWhiteSpace(Seller))
            {
                sales = sales.Where(s => s.Seller.UserFullName.Contains(Seller, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            // Filtro por departamentos
            if (DepartmentIds != null && DepartmentIds.Any())
            {
                sales = sales.Where(s => DepartmentIds.Contains(s.Seller.Department.Id)).ToList();
            }

            // Filtro por status de venda
            if (SalesStatusIds != null && SalesStatusIds.Any())
            {
                sales = sales.Where(s => SalesStatusIds.Contains(s.Status)).ToList();
            }

            // Filtro por data mínima
            if (!string.IsNullOrWhiteSpace(minDate.ToString()))
            {
                sales = sales.Where(s => s.Date >= minDate.Value).ToList();
            }

            // Filtro por data máxima
            if (!string.IsNullOrWhiteSpace(maxDate.ToString()))
            {
                sales = sales.Where(s => s.Date <= maxDate.Value).ToList();
            }

            return PartialView("_SalesTablePartial", sales);
        }

        public async Task<IActionResult> SimpleSearch(DateTime? minDate, DateTime? maxDate)
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



            var result = await _salesRecordService.FindByDateAsync(minDate, maxDate);
            return PartialView("_SalesRecordsTable", result);
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
            return PartialView("_SalesGroupRecordsTable", result);
        }

        public async Task<IActionResult> Create()
        {
            var sales = await _salesRecordService.FindAllByAsync();
            var sellers = await _sellerService.FindAllAsync();
            var departments = await _departmentService.FindAllAsync();
            var viewModel = new SalesFromViewModel
            {
                SalesRecord = new SalesRecord(),
                Sellers = sellers,
                Departments = departments
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SalesRecord salesRecord)
        {
            await _salesRecordService.InsertAsync(salesRecord);
            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Edit(int id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id not provided." });
            }
            var salesRecord = await _salesRecordService.FindByIdAsync(id);

            if (salesRecord == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Sales record not found." });
            }

            var departments = await _departmentService.FindAllAsync();
            var sellers = await _sellerService.FindAllAsync();

            var viewModel = new SalesFromViewModel
            {
                SalesRecord = salesRecord,
                Departments = departments,
                Sellers = sellers,
                Status = salesRecord.Status  // opcional, se usado em dropdown
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, SalesRecord salesRecord)
        {
            if (id != salesRecord.Id) return BadRequest();
            ModelState.Remove("SalesRecord.Seller");


            if (!ModelState.IsValid)
            {
                var departments = await _departmentService.FindAllAsync();
                var sellers = await _sellerService.FindAllAsync();
                var salesRecordOld = await _salesRecordService.FindByIdAsync(id);
                var viewModel = new SalesFromViewModel
                {
                    SalesRecord = salesRecordOld,
                    Departments = departments,
                    Sellers = sellers
                };
                Console.WriteLine("Model invalido");
                return View(viewModel);
            }

            try
            {
                await _salesRecordService.UpdateAsync(salesRecord);
                return RedirectToAction(nameof(Index));
            }
            catch (ApplicationException ex)
            {
                return RedirectToAction(nameof(Error), new { message = ex.Message });
                ;
            }
        }

        public async Task<IActionResult> Details(int id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id not provided." });
            }

            var departments = await _departmentService.FindAllAsync();
            var sellers = await _sellerService.FindAllAsync();
            var salesRecord = await _salesRecordService.FindByIdAsync(id);
            var viewModel = new SalesFromViewModel
            {
                SalesRecord = salesRecord,
                Departments = departments,
                Sellers = sellers
            };
            return PartialView("_details", viewModel);
        }

    }
}
