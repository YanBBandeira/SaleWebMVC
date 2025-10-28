using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SalesWebMVC.Models;
using SalesWebMVC.Models.ViewModels;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SalesWebMVC.Controllers
{
    public class ProfilesController : Controller
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public ProfilesController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Index()
        {
            var usersQuery = await _userManager.Users.AsQueryable().ToListAsync();

            var userList = new List<ProfilesIndexViewModel>();

            foreach (var user in usersQuery)
            {
                var roles = await _userManager.GetRolesAsync(user);
                userList.Add(new ProfilesIndexViewModel
                {
                    Id = user.Id,
                    Login = user.UserName,
                    Name = user.UserFullName,
                    Email = user.Email,
                    Role = roles.FirstOrDefault().ToString() ?? "None"
                });
            }
            ViewBag.Roles = await _roleManager.Roles
                .Select(r => r.Name)
                .ToListAsync();
            return View(userList);
        }


        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Filter(string UserFullName, string email, List<string> roles)
        {
            var usersQuery = _userManager.Users.AsQueryable();

            if (!string.IsNullOrEmpty(UserFullName))
            {
                usersQuery = usersQuery.Where(u => u.UserFullName.Contains(UserFullName));
            }
            if (!string.IsNullOrEmpty(email))
            {
                usersQuery = usersQuery.Where(u => u.Email.Contains(email));
            }
            if (roles != null && roles.Count > 0)
            {
                var usersWithRoles = new List<ApplicationUser>();
                foreach (var role in roles)
                {
                    var usersInRole = await _userManager.GetUsersInRoleAsync(role);
                    usersWithRoles.AddRange(usersInRole);
                }
                usersQuery = usersQuery.Where(u => usersWithRoles.Select(x => x.Id).Contains(u.Id));
            }

            var userList = new List<ProfilesIndexViewModel>();
            var users = await usersQuery.ToListAsync();

            foreach (var user in users)
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                userList.Add(new ProfilesIndexViewModel
                {
                    Id = user.Id,
                    Login = user.UserName,
                    Name = user.UserFullName,
                    Email = user.Email,
                    Role = userRoles.FirstOrDefault() ?? "None"
                });
            }

            return PartialView("_profilesTable",userList);
        }


        [HttpGet]
        [Authorize(Roles = "Admin,Manager")]
        public IActionResult Create()
        {
            ViewBag.Roles = _roleManager.Roles.Select(r => r.Name).ToList();
            // Exemplo: pega a primeira role do usuário
            ViewBag.UserRole = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProfilesCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Something went wrong!");
                return View(model);
            }

            var user = new ApplicationUser
            {
                UserName = model.Name,
                UserFullName = model.FullName,
                Email = model.Email,
                EmailConfirmed = true,
                BirthDate = model.BirthDate,
                BaseSalary = model.BaseSalary
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Error creating profile.");
                return View(model);
            }

            var roleExists = await _roleManager.RoleExistsAsync(model.Role);
            if (!roleExists)
            {
                return BadRequest("Role not valid");
            }

            await _userManager.AddToRoleAsync(user, model.Role);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            var model = new ProfilesEditViewModel
            {
                Id = user.Id,
                Name = user.UserName,
                FullName = user.UserFullName,
                Email = user.Email,
                Role = (await _userManager.GetRolesAsync(user)).FirstOrDefault() ?? "None",
                BirthDate = user.BirthDate,
                BaseSalary = user.BaseSalary
            };

            var currentUserId = _userManager.GetUserId(User);
            ViewBag.CurrentUserId = currentUserId;
            ViewBag.Roles = _roleManager.Roles.Select(r => r.Name).ToList();
            ViewBag.UserRole = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProfilesEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                Console.WriteLine("ALGO DEU ERRADO!");
                ModelState.AddModelError("", "Something went wrong!");
                return View(model);
            }

            var currentUserId = _userManager.GetUserId(User);
            var isSelfUpdate = currentUserId == model.Id;
            var isAdmin = User.IsInRole("Admin");
            var isManager = User.IsInRole("Manager");

            // Segurança: bloquear se não for dono do perfil nem admin
            if (!(isSelfUpdate || isAdmin || isManager))
            {
                return Forbid(); // 403
            }


            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Something went wrong!");
                return View(model);
            }

            var user = await _userManager.FindByIdAsync(model.Id);

            if (user == null)
            {
                return NotFound();
            }

            user.UserName = model.Name;
            user.UserFullName = model.FullName;
            user.Email = model.Email;
            user.BirthDate = model.BirthDate;
            user.BaseSalary = model.BaseSalary;

            // Atualizar roles corretamente
            var currentRoles = await _userManager.GetRolesAsync(user); // Pega as roles atuais do usuário
            await _userManager.RemoveFromRolesAsync(user, currentRoles);// Remove todas as roles atuais
            await _userManager.AddToRoleAsync(user, model.Role);// Adiciona a nova role

            var updateResult = await _userManager.UpdateAsync(user);// Atualiza o usuário

            if (!updateResult.Succeeded)
            {
                foreach (var error in updateResult.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(model);
            }

            // Se o campo Nova Senha foi preenchido
            Console.WriteLine("NewPassword: " + model.NewPassword);
            if (!string.IsNullOrWhiteSpace(model.NewPassword))
            {
                if (isSelfUpdate)
                {
                    // Para o próprio usuário, exige senha atual
                    if (string.IsNullOrWhiteSpace(model.CurrentPassword))
                    {
                        ModelState.AddModelError("CurrentPassword", "Senha atual obrigatória para alterar a senha.");
                        return View(model);
                    }

                    var changeResult = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);

                    if (!changeResult.Succeeded)
                    {
                        foreach (var error in changeResult.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                        return View(model);
                    }
                }
                else if (isAdmin)
                {
                    // Para admin, troca a senha sem exigir senha atual
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                    var resetResult = await _userManager.ResetPasswordAsync(user, token, model.NewPassword);

                    if (!resetResult.Succeeded)
                    {
                        foreach (var error in resetResult.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                        return View(model);
                    }
                }
                else
                {
                    // Caso raro: não admin tentando trocar senha de outro -> bloqueia
                    return Forbid();
                }
            }
            return RedirectToAction(nameof(Index));
        }



        [HttpPost]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id)) // Verifica se o ID é nulo ou vazio
            {
                return NotFound();
            }
            var user = await _userManager.FindByIdAsync(id); // Encontra o usuário pelo ID
            if (user == null)
            {
                return NotFound();
            }
            // Impede que usuários com papel "Admin" sejam excluídos
            var roles = await _userManager.GetRolesAsync(user); // Obtém as roles do usuário
            if (roles.Contains("Admin")) // Verifica se o usuário tem a role "Admin"
            {
                TempData["Error"] = "Usuários com perfil Admin não podem ser excluídos.";
                return RedirectToAction(nameof(Index));
            }

            var result = await _userManager.DeleteAsync(user); // Tenta excluir o usuário
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Error deleting profile.");
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Index));
        }

    }
}
