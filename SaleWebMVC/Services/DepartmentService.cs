using Microsoft.EntityFrameworkCore; //inserido para usar o ToListAsync
using SalesWebMVC.Data;
using SalesWebMVC.Models;

namespace SalesWebMVC.Services
{
    public class DepartmentService
    {
        private readonly ApplicationDbContext _context;

        public DepartmentService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Department>> FindAllAsync() //o sufixo Async é uma recomendação 
        {
            return await _context.Departments.OrderBy(x => x.Name).ToListAsync(); 
            // await foi inserido por estarmos trabalhando com uma ação assícrona. 
        }
    }
}
