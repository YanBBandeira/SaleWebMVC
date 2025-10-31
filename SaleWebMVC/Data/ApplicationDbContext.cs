using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SalesWebMVC.Models;

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
        

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ApplicationUser>()
                .HasOne(u => u.Department)
                .WithMany(d => d.Sellers)
                .HasForeignKey(u => u.DepartmentId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(false);
        }

    }
}
