using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SalesWebMVC.Data;
using System.Configuration;
var builder = WebApplication.CreateBuilder(args);
//builder.Services.AddDbContext<SaleWebMVCContext>(options =>
//    options.UseMySql(builder.Configuration.GetConnectionString("SaleWebMVCContext") ?? throw new InvalidOperationException("Connection string 'SaleWebMVCContext' not found.")));
builder.Services.AddDbContext<SalesWebMVCContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("SalesWebMVCContext"), ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("SalesWebMVCContext")), builder =>
    builder.MigrationsAssembly("SalesWebMVC")));
builder.Services.AddScoped<SeedingService>();
// Add services to the container.
builder.Services.AddControllersWithViews();
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        var seedingService = services.GetRequiredService<SeedingService>();
        seedingService.Seed(); // aqui você popula o banco
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
