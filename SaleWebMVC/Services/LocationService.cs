using SalesWebMVC.Data;
using SalesWebMVC.Models;

namespace SalesWebMVC.Services
{
    public class LocationService
    {
        private readonly ApplicationDbContext _context;

        public LocationService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<State>> FindAllStatesAsync()
        {
            return _context.State.OrderBy(s => s.Name).ToList();
        }

        public async Task<State> FindStateByIdAsync(int id)
        {
            return await _context.State.FindAsync(id);
        }

        public async Task<List<City>> FindAllCitiesAsync()
        {
            return _context.City.OrderBy(c => c.Name).ToList();
        }
        public async Task<List<City>> FindCitiesByStateIdAsync(int stateId)
        {
            return _context.City
                .Where(c => c.StateId == stateId)
                .OrderBy(c => c.Name).ToList();
        }

    }
}
