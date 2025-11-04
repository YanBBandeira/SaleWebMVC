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

        public DbSet<OrderItem> OrderItems { get; set; }

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

            // 3. Configuração do Relacionamento SalesRecord <-> OrderItem (CRÍTICO!)
            builder.Entity<SalesRecord>()
                .HasMany(s => s.OrderItems)          // SalesRecord tem muitos OrderItems
                .WithOne(i => i.SalesRecord)        // OrderItem pertence a um SalesRecord
                .HasForeignKey(i => i.SalesRecordId) // A FK é o campo SalesRecordId no OrderItem
                .OnDelete(DeleteBehavior.Cascade);   // Se a Venda for deletada, os Itens também são
        }

    }
}
