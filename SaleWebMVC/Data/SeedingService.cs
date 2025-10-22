using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Plugins;
using SalesWebMVC.Models;
using SalesWebMVC.Models.Enums;
using SalesWebMVC.Services;

namespace SalesWebMVC.Data
{
    public class SeedingService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<SeedingService> _logger;

        public SeedingService(ApplicationDbContext context,
                           UserManager<ApplicationUser> userManager,
                           RoleManager<IdentityRole> roleManager,
                           ILogger<SeedingService> logger)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;
        }


        public async Task SeedAsync()
        {

            await _context.Database.EnsureCreatedAsync();

            if (_context.Departments.Any() || _userManager.Users.Any() || _context.SalesRecords.Any())
            {
                _logger.LogInformation("Database already seeded.");
                return;
            }

            await CreateRolesAsync();

            await CreateAdminUserAsync();

            


            // 1. Criar departamentos
            var d1 = new Department { Name = "Computers" };
            var d2 = new Department { Name = "Electronics" };
            var d3 = new Department { Name = "Fashion" };
            var d4 = new Department { Name = "Books" };


            _context.Departments.AddRange(d1, d2, d3, d4);
            await _context.SaveChangesAsync();
           
            //// 2. Criar roles
            //string[] roles = { "Admin", "Manager", "Seller" };
            //foreach (var role in roles)
            //{
            //    if (!await _roleManager.RoleExistsAsync(role))
            //    {
            //        await _roleManager.CreateAsync(new IdentityRole(role));
            //    }
            //}
            // 3. Criar usuários vendedores
            var sellers = new List<ApplicationUser>
            {
                new ApplicationUser
                {
                    UserName = "bob.brown",
                    Email = "bob@gmail.com",
                    UserFullName = "Bob Brown",
                    BirthDate = new DateTime(1998, 4, 21),
                    BaseSalary = 1000,
                    DepartmentId = d1.Id,
                    EmailConfirmed = true
                },
                new ApplicationUser
                {
                    UserName = "maria.green",
                    Email = "maria@gmail.com",
                    UserFullName = "Maria Green",
                    BirthDate = new DateTime(1979, 12, 31),
                    BaseSalary = 3500,
                    DepartmentId = d2.Id,
                    EmailConfirmed = true
                },
                new ApplicationUser
                {
                    UserName = "alex.grey",
                    Email = "alex@gmail.com",
                    UserFullName = "Alex Grey",
                    BirthDate = new DateTime(1988, 1, 15),
                    BaseSalary = 2200,
                    DepartmentId = d1.Id,
                    EmailConfirmed = true
                },
                new ApplicationUser
                {
                    UserName = "martha.red",
                    Email = "martha@gmail.com",
                    UserFullName = "Martha Red",
                    BirthDate = new DateTime(1993, 11, 30),
                    BaseSalary = 3000,
                    DepartmentId = d4.Id,
                    EmailConfirmed = true
                },
                new ApplicationUser
                {
                    UserName = "donald.blue",
                    Email = "donald@gmail.com",
                    UserFullName = "Donald Blue",
                    BirthDate = new DateTime(2000, 1, 9),
                    BaseSalary = 4000,
                    DepartmentId = d3.Id,
                    EmailConfirmed = true
                },
                new ApplicationUser
                {
                    UserName = "alex.pink",
                    Email = "pink@gmail.com",
                    UserFullName = "Alex Pink",
                    BirthDate = new DateTime(1997, 3, 4),
                    BaseSalary = 3000,
                    DepartmentId = d2.Id,
                    EmailConfirmed = true
                },
            };


            foreach (var seller in sellers)
            {
                var exists = await _userManager.FindByNameAsync(seller.UserName);
                if (exists == null)
                {
                    var result = await _userManager.CreateAsync(seller, "@Senha123");
                    if (result.Succeeded)
                    {
                        await _userManager.AddToRoleAsync(seller, "Seller");
                    }
                    else
                    {
                        _logger.LogError($"Erro ao criar usuário {seller.UserName}: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                    }
                }
            }

            await _context.SaveChangesAsync();

            // 4. Recarregar usuários do DB (para ter IDs)
            var allUsers = await _userManager.Users.ToListAsync();

            var s1 = allUsers.First(u => u.UserName == "bob.brown");
            var s2 = allUsers.First(u => u.UserName == "maria.green");
            var s3 = allUsers.First(u => u.UserName == "alex.grey");
            var s4 = allUsers.First(u => u.UserName == "martha.red");
            var s5 = allUsers.First(u => u.UserName == "donald.blue");
            var s6 = allUsers.First(u => u.UserName == "alex.pink");



            // 5. Criar SalesRecords
            var sales = new List<SalesRecord>
            {
                new SalesRecord(1, new DateTime(2025,08,25), 11000.0, SalesStatus.Billed, s1),
                new SalesRecord(2, new DateTime(2025,08,04), 7000.0, SalesStatus.Billed, s5),
                new SalesRecord(3, new DateTime(2025,08,13), 4000.0, SalesStatus.Canceled, s4),
                new SalesRecord(4, new DateTime(2025,08,01), 8000.0, SalesStatus.Billed, s1),
                new SalesRecord(5, new DateTime(2025,08,21), 3000.0, SalesStatus.Billed, s3),
                new SalesRecord(6, new DateTime(2025,08,15), 2000.0, SalesStatus.Billed, s1),
                new SalesRecord(7, new DateTime(2025,08,28), 13000.0, SalesStatus.Billed, s2),
                new SalesRecord(8, new DateTime(2025,08,11), 4000.0, SalesStatus.Billed, s4),
                new SalesRecord(9, new DateTime(2025,08,14), 11000.0, SalesStatus.Pending, s6),
                new SalesRecord(10, new DateTime(2025,08,07), 9000.0, SalesStatus.Billed, s6),
                new SalesRecord(11, new DateTime(2025,08,13), 6000.0, SalesStatus.Billed, s2),
                new SalesRecord(12, new DateTime(2025,08,25), 7000.0, SalesStatus.Pending, s3),
                new SalesRecord(13, new DateTime(2025,08,29), 10000.0, SalesStatus.Billed, s4),
                new SalesRecord(14, new DateTime(2025,08,04), 3000.0, SalesStatus.Billed, s5),
                new SalesRecord(15, new DateTime(2025,08,12), 4000.0, SalesStatus.Billed, s1),
                new SalesRecord(16, new DateTime(2025,09,05), 2000.0, SalesStatus.Billed, s4),
                new SalesRecord(17, new DateTime(2025,09,01), 12000.0, SalesStatus.Billed, s1),
                new SalesRecord(18, new DateTime(2025,09,24), 6000.0, SalesStatus.Billed, s3),
                new SalesRecord(19, new DateTime(2025,09,22), 8000.0, SalesStatus.Billed, s5),
                new SalesRecord(20, new DateTime(2025,09,15), 8000.0, SalesStatus.Billed, s6),
                new SalesRecord(21, new DateTime(2025,09,17), 9000.0, SalesStatus.Billed, s2),
                new SalesRecord(22, new DateTime(2025,09,24), 4000.0, SalesStatus.Billed, s4),
                new SalesRecord(23, new DateTime(2025,09,19), 11000.0, SalesStatus.Canceled, s2),
                new SalesRecord(24, new DateTime(2025,09,12), 8000.0, SalesStatus.Billed, s5),
                new SalesRecord(25, new DateTime(2025,09,30), 7000.0, SalesStatus.Billed, s3),
                 new SalesRecord(26, new DateTime(2025,09,06), 5000.0, SalesStatus.Billed, s4),
                 new SalesRecord(27, new DateTime(2025,09,13), 9000.0, SalesStatus.Pending, s1),
                 new SalesRecord(28, new DateTime(2025,09,07), 4000.0, SalesStatus.Billed, s3),
                 new SalesRecord(29, new DateTime(2025,09,23), 12000.0, SalesStatus.Billed, s5),
                 new SalesRecord(30, new DateTime(2025,09,12), 5000.0, SalesStatus.Billed, s2)
             };

            _context.SalesRecords.AddRange(sales);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Database seeded successfully.");
        }

        private async Task CreateRolesAsync()
        {
            string[] roles = { "Admin", "Manager", "Seller", "Sales manager" };
            foreach (var roleName in roles)
            {
                if (!await _roleManager.RoleExistsAsync(roleName))
                {
                    var result = await _roleManager.CreateAsync(new IdentityRole(roleName));
                    if (!result.Succeeded)
                    {
                        _logger.LogError($"Failed to create role '{roleName}': {string.Join(", ", result.Errors.Select(e => e.Description))}");
                    }
                }
            }
        }

        private async Task CreateAdminUserAsync()
        {
            var adminName = "admin";
            if (await _userManager.FindByNameAsync(adminName) == null)
            {
                var adminUser = new ApplicationUser
                {
                    UserName = adminName,
                    Email = "admin@codehub.com",
                    UserFullName = "Administrator",
                    EmailConfirmed = true,
                    SecurityStamp = Guid.NewGuid().ToString()
                };

                var result = await _userManager.CreateAsync(adminUser, "Admin@123");
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(adminUser, "Admin");
                    _logger.LogInformation("Admin user created and assigned to Admin role.");
                }
                else
                {
                    _logger.LogError($"Failed to create admin user: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                }
            }
            else
            {
                _logger.LogInformation("Admin user already exists.");
            }
        }
    }
}
