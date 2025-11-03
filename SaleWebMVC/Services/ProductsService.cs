using Microsoft.EntityFrameworkCore;
using SalesWebMVC.Data;
using SalesWebMVC.Models;

namespace SalesWebMVC.Services
{
    public class ProductsService
    {
        private readonly ApplicationDbContext _context;

        public ProductsService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Product>> FindAllAsync()
        {
            return await _context.Products.Include(p => p.Department)
                .Include(p => p.Supplier)
                .ThenInclude(p => p.City)
                .ThenInclude(p => p.State)
                .ToListAsync();
        }
        public async Task<List<Product>> FindAllWithSupplirLocationAsync()
        {
            return await _context.Products.Include(p => p.Department)
                                           .Include(p => p.Supplier)
                                          .ThenInclude(s => s.City)
                                          .ThenInclude(c => c.State)
                                          .ToListAsync();
        }

        public async Task<Product> FindByIdAsync(int id)
        {
            return await _context.Products.Include(p => p.Department).Include(p => p.Supplier)
                                          .ThenInclude(s => s.City)
                                          .ThenInclude(c => c.State)
                                          .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<List<Product>> FindBySupplierIdAsync(int supplierId)
        {
            return await _context.Products.Include(p => p.Department).Include(p => p.Supplier)
                                          .ThenInclude(s => s.City)
                                          .ThenInclude(c => c.State)
                                          .Where(p => p.SupplierId == supplierId)
                                          .ToListAsync();
        }

        public async Task<List<Product>> FindByDepartmentIdAsync(int departmentId)
        {
            return await _context.Products.Include(p => p.Department).Include(p => p.Supplier)
                                          .ThenInclude(s => s.City)
                                          .ThenInclude(c => c.State)
                                          .Where(p => p.DepartmentId == departmentId)
                                          .ToListAsync();
        }
        // Inserir produto
        public async Task AddProductAsync(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
        }

        // Atualizar produto
        public async Task UpdateProductAsync(Product product)
        {
            bool exists = await _context.Products.AnyAsync(p => p.Id == product.Id);
            if (!exists)
            {
                throw new KeyNotFoundException($"Product with Id {product.Id} not found");
            }
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }

        // Remover produto
        public async Task DeleteProductAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                throw new KeyNotFoundException($"Product with Id {id} not found");
            }
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }
    }
}
