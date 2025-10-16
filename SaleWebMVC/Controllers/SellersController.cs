using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Plugins;
using SalesWebMVC.Models;
using SalesWebMVC.Models.ViewModels;
using SalesWebMVC.Services;
using SalesWebMVC.Services.Exceptions;
using System.Diagnostics;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace SalesWebMVC.Controllers
{
    public class SellersController : Controller
    {
        private readonly SellerService _sellerService;
        private readonly DepartmentService _departmentService;
        private readonly UserManager<ApplicationUser> _userManager;

        public SellersController(SellerService sellerService, DepartmentService departmentService, UserManager<ApplicationUser> userManager)
        {
            _sellerService = sellerService;
            _departmentService = departmentService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var list = await _sellerService.FindAllAsync();
            return View(list);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string id, ApplicationUser user)
        {
            if (id != user.Id)
            {
                return RedirectToAction(nameof(Error), new { message = "Id mismatch" });
            }

            try
            {
                // Atualiza só os dados extras do seller (não cria usuário, nem altera login/email)
                await _sellerService.UpdateSellerExtrasAsync(user);
                return RedirectToAction(nameof(Index));
            }
            catch (ApplicationException ex)
            {
                return RedirectToAction(nameof(Error), new { message = ex.Message });
            }
        }


        public async Task<IActionResult> Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
                return RedirectToAction(nameof(Error), new { message = "Id not provided." });

            var obj = await _sellerService.FindByIdAsync(id);
            if (obj == null)
                return RedirectToAction(nameof(Error), new { message = "Id not found." });

            var departments = await _departmentService.FindAllAsync();
            var viewModel = new SellerFormViewModel { Seller = obj, Departments = departments };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, ApplicationUser user)
        {
            if (id != user.Id)
            {
                return RedirectToAction(nameof(Error), new { message = "Id mismatch" });
            }

            try
            {
                // Atualiza só os dados extras do seller (não cria usuário, nem altera login/email)
                await _sellerService.UpdateSellerExtrasAsync(user);
                return RedirectToAction(nameof(Index));
            }
            catch (ApplicationException ex)
            {
                return RedirectToAction(nameof(Error), new { message = ex.Message });
            }
        }


        public IActionResult Error(string message)
        {
            var viewModel = new ErrorViewModel
            {
                Message = message,
                RequestId = Activity.Current?.Id
            };
            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> GetSellersByDepartment(int departmentId)
        {
            try
            {
                var sellers = await _sellerService.FindByDepartmentIdAsync(departmentId);
                var result = sellers.Select(s => new
                {
                    id = s.Id,
                    userFullName = s.UserFullName // Corrigido para compatibilidade com a View
                }).ToList();

                return Json(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in GetSellersByDepartment: " + ex.Message);
                return StatusCode(500, "Server error");
            }
        }
    }
}
