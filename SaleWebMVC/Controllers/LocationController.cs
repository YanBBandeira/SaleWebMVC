using Microsoft.AspNetCore.Mvc;
using SalesWebMVC.Services;

namespace SalesWebMVC.Controllers
{

    [Route("api/[controller]")] // Define a rota base como /api/Location
    [ApiController]
    public class LocationController : ControllerBase // NOTA: Deve herdar de ControllerBase para APIs
    {


        private readonly LocationService _locationService;

        public LocationController(LocationService locationService)
        {
            _locationService = locationService;
        }


        // GET: api/Location/CitiesByState?stateId=X
        [HttpGet("CitiesByState")]
        public async Task<IActionResult> CitiesByState(int stateId)
        {
            if (stateId <= 0)
            {
                return BadRequest();
            }

            var cities = await _locationService.FindCitiesByStateIdAsync(stateId);

            // Retorna a lista de cidades em formato JSON
            return Ok(cities.Select(c => new { id = c.Id, name = c.Name }));
        }

    }
}
