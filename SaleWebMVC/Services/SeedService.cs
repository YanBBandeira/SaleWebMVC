using Microsoft.AspNetCore.Identity;
using SalesWebMVC.Data;
using SalesWebMVC.Models;

namespace SalesWebMVC.Services
{
    public class SeedService
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<SalesWebMVCContext>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<SeedService>>();

            try
            {
                logger.LogInformation("Starting database seeding...");
                await context.Database.EnsureCreatedAsync(); // Ensure database is created

                logger.LogInformation("Checking and creating roles...");
                await AddRoleAsync(roleManager, "Admin"); // Add Admin role
                await AddRoleAsync(roleManager, "Manager");  // Add User role
                await AddRoleAsync(roleManager, "Seller");  // Add User role
                await AddRoleAsync(roleManager, "Sales manager");  // Add User role

                logger.LogInformation("Checking and creating admin and other users...");
                var adminName = "admin";
                if (await userManager.FindByNameAsync(adminName) == null)
                {
                    var adminUser = new ApplicationUser
                    {
                        UserName = adminName, // Default username
                        Email = "admin@codehub.com", // Default email
                        UserFullName = "Administrator", // Full name
                        NormalizedEmail = "admin@codehub.com".ToUpper(), // Normalized email
                        NormalizedUserName = adminName.ToUpper(), // Normalized username
                        EmailConfirmed = true, // Email confirmed
                        SecurityStamp = Guid.NewGuid().ToString() // Security stamp
                    };
                    var result = await userManager.CreateAsync(adminUser, "Admin@123"); // Default password
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(adminUser, "Admin");
                        logger.LogInformation("Admin user created and assigned to Admin role.");
                    }
                    else
                    {
                        logger.LogError("Failed to create admin user: {Errors}", string.Join(", ", result.Errors.Select(e => e.Description)));
                    }
                }
                else
                {
                    logger.LogInformation("Admin user already exists.");
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while seeding the database.");
            }
        }

        private static async Task AddRoleAsync(RoleManager<IdentityRole> roleManager, string roleName)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                var result = await roleManager.CreateAsync(new IdentityRole(roleName));
                if (!result.Succeeded)
                {
                    throw new Exception($"Failed to create role '{roleName}': {string.Join(", ", result.Errors.Select(e => e.Description))}");
                }
            }
        }
    }
}
