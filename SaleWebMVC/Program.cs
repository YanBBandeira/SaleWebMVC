using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SalesWebMVC.Data;
using SalesWebMVC.Models;
using SalesWebMVC.Services;
using System.Configuration;
using System.Globalization;
using System.Threading.Tasks;
internal class Program
{
    private static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseMySql(builder.Configuration.GetConnectionString("ApplicationDbContext"),
            ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("ApplicationDbContext")),
            builder => builder.MigrationsAssembly("SalesWebMVC")));

        builder.Services.AddScoped<SeedingService>();
        builder.Services.AddScoped<SellerService>();
        builder.Services.AddScoped<DepartmentService>();
        builder.Services.AddScoped<SalesRecordService>();
        builder.Services.AddScoped<StatsService>();
        // Add services to the container.
        builder.Services.AddControllersWithViews();
        builder.Services.AddDefaultIdentity<ApplicationUser>(
            options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 6;
                options.SignIn.RequireConfirmedAccount = false;
            }
        )
        .AddRoles<IdentityRole>() // habilita perfis
        .AddDefaultTokenProviders() // habilita a funcionalidade de reset de senha
        .AddEntityFrameworkStores<ApplicationDbContext>(); // define o ApplicationDbContext como o banco de dados de identidade

        builder.Services.ConfigureApplicationCookie(options =>
        {
            options.LoginPath = "/Account/Login"; // Se o usuário não estiver logado
            options.AccessDeniedPath = "/Account/AccessDenied"; // Se não tiver permissão
        });

        var app = builder.Build(); // constrói o aplicativo

        // SEEDING DE DADOS (corrigido):
        using (var scope = app.Services.CreateScope())
        {
            var seedingService = scope.ServiceProvider.GetRequiredService<SeedingService>();
            try
            {
                await seedingService.SeedAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Program: Error: An error occurred seeding the DB.\n{e.Message}");
            }
        }



        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection(); // Redireciona HTTP para HTTPS
        app.UseStaticFiles(); // Habilita o uso de arquivos estáticos (CSS, JS, imagens, etc.)


        //Locale
        var supportedCultures = new[]
        {
            new CultureInfo("en-US"),
            new CultureInfo("pt-BR")
        };

        var localizationOptions = new RequestLocalizationOptions
        {
            DefaultRequestCulture = new RequestCulture("pt-BR"),
            SupportedCultures = supportedCultures,
            SupportedUICultures = supportedCultures
        }; // Configurações de localização

        app.UseRequestLocalization(localizationOptions); // Adicione esta linha para habilitar a localização

        app.UseRouting(); // Mantenha esta linha para habilitar o roteamento

        app.UseAuthentication(); // Adicione esta linha para habilitar a autenticação

        app.UseAuthorization(); // Mantenha esta linha para habilitar a autorização

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}"); // Rota padrão
            //pattern: "{controller=Account}/{action=Login}/{id?}");
        app.Run(); // Inicia o aplicativo
    }
}