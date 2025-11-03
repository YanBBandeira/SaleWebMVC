using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SalesWebMVC.Models;
using System.Reflection.Emit;

namespace SalesWebMVC.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Department> Departments { get; set; }
        public DbSet<SalesRecord> SalesRecords { get; set; }

        public DbSet<State> State { get; set; }
        public DbSet<City> City { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Product> Products { get; set; }

        public DbSet<InventoryMovement> InventoryMovements { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ApplicationUser>()
                .HasOne(u => u.Department)
                .WithMany(d => d.Sellers)
                .HasForeignKey(u => u.DepartmentId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(false);

            // Data/SaleWebMVCContext.cs (Dentro do OnModelCreating)

            // 1. Configuração do Relacionamento com DEPARTMENT (CORREÇÃO)
            builder.Entity<Product>()
                .HasOne(p => p.Department)
                .WithMany() // <--- SEM ARGUMENTO: Relacionamento unidirecional
                .HasForeignKey(p => p.DepartmentId)
                .IsRequired();

            // 2. Configuração do Relacionamento com SUPPLIER (CORREÇÃO)
            builder.Entity<Product>()
                .HasOne(p => p.Supplier)
                .WithMany(s => s.Products) // <--- SEM ARGUMENTO: Relacionamento unidirecional
                .HasForeignKey(p => p.SupplierId)
                .IsRequired();
        }

    }
}
