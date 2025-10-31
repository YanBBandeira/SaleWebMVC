using SalesWebMVC.Models;
using SalesWebMVC.Data;
using Microsoft.EntityFrameworkCore;

namespace SalesWebMVC.Services
{
    public class SuppliersService
    {
        private readonly ApplicationDbContext _context;
        public SuppliersService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<List<Supplier>> FindAllAsync()
        {
            return await _context.Suppliers
                .Include(s => s.City)
                .ThenInclude(c => c.State)
                .ToListAsync();
        }

        //Create 
        public Task InsertAsync(Supplier obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(nameof(obj));
            }
            _context.Add(obj);
            return _context.SaveChangesAsync();
        }

        //Read by Id
        public async Task<Supplier> FindByIdAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Invalid supplier ID", nameof(id));
            }
            return await _context.Suppliers
                .Include(s => s.City)
                .ThenInclude(c => c.State)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        // Update
        public async Task UpdateAsync(Supplier obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(nameof(obj));
            }
            bool exists = await _context.Suppliers.AnyAsync(s => s.Id == obj.Id);
            if (!exists)
            {
                throw new KeyNotFoundException($"Supplier with Id {obj.Id} not found");
            }
            _context.Suppliers.Update(obj);
            await _context.SaveChangesAsync();
        }


        // Delete
        public async Task RemoveAsync(int id)
        {
            var supplier = await _context.Suppliers.FindAsync(id);
            if (supplier == null)
            {
                throw new KeyNotFoundException($"Supplier with Id {id} not found");
            }
            _context.Suppliers.Remove(supplier);
            await _context.SaveChangesAsync();
        }
    }
}
