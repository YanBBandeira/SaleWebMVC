using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Plugins;
using SalesWebMVC.Data;
using SalesWebMVC.Models;
using SalesWebMVC.Services.Exceptions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesWebMVC.Services
{
    public class SellerService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public SellerService(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }


        // Retorna somente os usuários com a role "Seller", incluindo o Departamento
        public async Task<List<ApplicationUser>> FindAllAsync()
        {
            var sellers = await _userManager.GetUsersInRoleAsync("Seller");

            // Carregar o Department manualmente (porque GetUsersInRoleAsync não faz Include)
            foreach (var seller in sellers)
            {
                _context.Entry(seller).Reference(s => s.Department).Load();
            }

            return sellers.ToList();
        }

        // Inserir info de um novo seller (usuário)

        public async Task UpdateSellerExtrasAsync(ApplicationUser user)
        {
            var userToUpdate = await _context.Users
                .Include(u => u.Department)
                .FirstOrDefaultAsync(u => u.Id == user.Id);

            if (userToUpdate == null)
            {
                throw new NotFoundException("User not found.");
            }

            if (!await _userManager.IsInRoleAsync(userToUpdate, "Seller"))
            {
                throw new ApplicationException("User is not in Seller role.");
            }

            // Atualiza só os campos extras (se vierem preenchidos)
            if (!string.IsNullOrEmpty(user.UserFullName))
            {
                userToUpdate.UserFullName = user.UserFullName;
            }

            if (user.BirthDate != default(DateTime))
            {
                userToUpdate.BirthDate = user.BirthDate;
            }

            if (user.BaseSalary != 0)
            {
                userToUpdate.BaseSalary = user.BaseSalary;
            }

            if (user.DepartmentId != 0)
            {
                userToUpdate.DepartmentId = user.DepartmentId;
            }

            // Atualiza no banco
            _context.Update(userToUpdate);
            await _context.SaveChangesAsync();
        }

        public async Task<ApplicationUser> FindByIdAsync(string id)
        {
            var user = await _context.Users
                .Include(u => u.Department)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user != null && await _userManager.IsInRoleAsync(user, "Seller"))
            {
                return user;
            }

            return null;
        }

        // Remover seller (ApplicationUser.Id é string)
        public async Task RemoveAsync(string id)
        {
            try
            {
                var user = await _context.Users.FindAsync(id);
                if (user == null || !(await _userManager.IsInRoleAsync(user, "Seller")))
                {
                    throw new NotFoundException("Seller not found");
                }

                await _userManager.DeleteAsync(user);
            }
            catch (DbUpdateException ex)
            {
                throw new IntegrityException(ex.Message);
            }
        }

        // Atualizar seller
        public async Task UpdateAsync(ApplicationUser user)
        {
            var exists = await _context.Users.AnyAsync(u => u.Id == user.Id);
            if (!exists)
            {
                throw new NotFoundException("Id not found");
            }

            try
            {
                _context.Update(user);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw new DbConcurrencyException(ex.Message);
            }
        }

        // Filtrar sellers por departamento
        public async Task<List<ApplicationUser>> FindByDepartmentIdAsync(int? departmentId)
        {
            var users = await _context.Users
                .Include(u => u.Department)
                .ToListAsync();

            var sellers = new List<ApplicationUser>();

            foreach (var user in users)
            {
                if (await _userManager.IsInRoleAsync(user, "Seller") &&
                    (!departmentId.HasValue || user.DepartmentId == departmentId))
                {
                    sellers.Add(user);
                }
            }

            return sellers;
        }
    }
}
