using Microsoft.EntityFrameworkCore; //inserido para usar o ToListAsync
using SalesWebMVC.Data;
using SalesWebMVC.Models;

namespace SalesWebMVC.Services
{
    public class DepartmentService
    {
        private readonly SalesWebMVCContext _context;

        public DepartmentService(SalesWebMVCContext context)
        {
            _context = context;
        }

        public async Task<List<Department>> FindAllAsync() //o sufixo Async é uma recomendação 
        {
            return await _context.Department.OrderBy(x => x.Name).ToListAsync(); 
            // await foi inserido por estarmos trabalhando com uma ação assícrona. 
        }
    }
}
