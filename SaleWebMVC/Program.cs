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

        builder.Services.AddDbContext<SalesWebMVCContext>(options => options.UseMySql(builder.Configuration.GetConnectionString("SalesWebMVCContext"),
            ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("SalesWebMVCContext")),
            builder => builder.MigrationsAssembly("SalesWebMVC")));

        builder.Services.AddScoped<SeedingService>();
        builder.Services.AddScoped<SellerService>();
        builder.Services.AddScoped<DepartmentService>();
        builder.Services.AddScoped<SalesRecordService>();
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
        .AddEntityFrameworkStores<SalesWebMVCContext>(); // define o SalesWebMVCContext como o banco de dados de identidade

        builder.Services.ConfigureApplicationCookie(options =>
        {
            options.LoginPath = "/Account/Login"; // Se o usu�rio n�o estiver logado
            options.AccessDeniedPath = "/Account/AccessDenied"; // Se n�o tiver permiss�o
        });

        var app = builder.Build(); // constr�i o aplicativo

        await SeedService.SeedAsync(app.Services); // Chama o m�todo SeedAsync para popular o banco de dados

        if (app.Environment.IsDevelopment())
        {
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var seedingService = services.GetRequiredService<SeedingService>();
                seedingService.Seed(); // aqui voc� popula o banco
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
        app.UseStaticFiles(); // Habilita o uso de arquivos est�ticos (CSS, JS, imagens, etc.)


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
        }; // Configura��es de localiza��o

        app.UseRequestLocalization(localizationOptions); // Adicione esta linha para habilitar a localiza��o

        app.UseRouting(); // Mantenha esta linha para habilitar o roteamento

        app.UseAuthentication(); // Adicione esta linha para habilitar a autentica��o

        app.UseAuthorization(); // Mantenha esta linha para habilitar a autoriza��o

        app.MapControllerRoute(
            name: "default",
           pattern: "{controller=Home}/{action=Index}/{id?}"); // Rota padr�o
           // pattern: "{controller=Account}/{action=Login}/{id?}");
        app.Run(); // Inicia o aplicativo
    }
}