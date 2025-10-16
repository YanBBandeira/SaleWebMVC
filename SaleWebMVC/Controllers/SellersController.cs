using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; //inserido para usar o ToListAsync
using NuGet.Protocol.Plugins;
using SalesWebMVC.Models;
using SalesWebMVC.Models.ViewModels;
using SalesWebMVC.Services;
using SalesWebMVC.Services.Exceptions;
using System.Diagnostics;
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

        public async Task<IActionResult> Create()
        {
            ViewBag.Users = await _userManager.Users.AsQueryable().ToListAsync();
            var departments = await _departmentService.FindAllAsync();
            var viewModel = new SellerFormViewModel { Departments = departments };
            return View(viewModel);
        }

        //Anotation
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Seller seller)
        {
            await _sellerService.InsertAsync(seller);
            return RedirectToAction(nameof(Index));
        }

        //A ? é para indicar que é opicional
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id not provided." });
            }
            // comom id é nullable, temos que colocar o método value
            var obj = await _sellerService.FindByIdAsync(id.Value);

            if (obj == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id not found." });
            }

            return View(obj);
        }


        //Anotation
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _sellerService.RemoveAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch (IntegrityException ex)
            {
                return RedirectToAction(nameof(Error), new { message = ex.Message });
            }
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id not provided." });
            }
            // comom id é nullable, temos que colocar o método value
            var obj = await _sellerService.FindByIdAsync(id.Value);

            if (obj == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id not found." });
            }

            return View(obj);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id not provided." });
            }

            var obj = await _sellerService.FindByIdAsync(id.Value);

            if (obj == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id not found." });
            }

            List<Department> departments = await _departmentService.FindAllAsync();
            SellerFormViewModel viewModel = new SellerFormViewModel { Seller = obj, Departments = departments };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Seller seller)
        {

            if (id != seller.Id)
            {
                return RedirectToAction(nameof(Error), new { message = "Id mismatch" });
            }
            try
            {
                await _sellerService.UpdateAsync(seller);
                return RedirectToAction(nameof(Index));
            }
            catch (ApplicationException ex)
            {
                return RedirectToAction(nameof(Error), new { message = ex.Message });
                ;
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

        // Exemplo em SellersController.cs (ou onde você lida com Sellers)

        [HttpGet]
        public async Task<IActionResult> GetSellersByDepartment(int departmentId)
        {
            try
            {
                //Console.WriteLine("Recebido departmentId: " + departmentId);
                var sellers = await _sellerService.FindByDepartmentIdAsync(departmentId);
                Console.WriteLine(JsonSerializer.Serialize(sellers));
                // Projeta o resultado para um formato JSON mais limpo (opcional, mas recomendado)
                var result = sellers.Select(s => new
                {
                    id = s.Id,
                    name = s.Name
                }).ToList();
                //Console.WriteLine(result.ToString());
                // Retorna a lista como JSON. A função AJAX do jQuery irá consumir isso.
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
