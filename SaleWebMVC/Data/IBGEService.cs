using Microsoft.AspNetCore.Authentication;
using Mono.TextTemplating;
using SalesWebMVC.Models;
using State = SalesWebMVC.Models.State;

namespace SalesWebMVC.Data
{
    public class IBGEService
    {
        private readonly ApplicationDbContext _context;
        private readonly HttpClient _httpClient;

        public IBGEService(ApplicationDbContext context, HttpClient httpClient)
        {
            _context = context;
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://servicodados.ibge.gov.br/api/v1/localidades/");
        }

        public async Task ImportStatesAndCitiesAsync()
        {
            // Verifica se já existem estados no banco de dados
            if( _context.City.Any() || _context.State.Any())
            {
                return; // Já existem estados, não precisa importar
            }

            // Faz a requisição para a API do IBGE para obter os estados
            var ibgeStates = await _httpClient.GetFromJsonAsync<List<IbgeStateModel>>("estados");
            if (ibgeStates == null)
            {
                throw new Exception("Não foi possível obter os dados dos estados do IBGE.");
            }

            var states = ibgeStates
                .Select( s => new State
                {
                    Id = s.Id,
                    Name = s.Nome,
                    UF = s.Sigla
                })
                .ToList();

            await _context.State.AddRangeAsync(states);
            await _context.SaveChangesAsync();


            // Importa as cidades para cada estado
            var allCities = new List<City>();
            foreach (var state in ibgeStates)
            {
                // busca cidades do estado
                var citiesUrl = $"estados/{state.Id}/municipios";
                var ibgeCities = await _httpClient.GetFromJsonAsync<List<IbgeCityModel>>(citiesUrl);

                if (ibgeCities == null)
                {
                    throw new Exception($"Não foi possível obter os dados das cidades do estado {state.Nome} do IBGE.");
                }

                var cities = ibgeCities
                    .Select( c => new City
                    {
                        Id = c.Id,
                        Name = c.Nome,
                        StateId = state.Id // Usa o ID do estado que já salvamos
                    })
                    .ToList();
                allCities.AddRange(cities);
            }

            await _context.City.AddRangeAsync(allCities);
            await _context.SaveChangesAsync();
        }
    }


    public class IbgeStateModel
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Sigla { get; set; }
    }

    public class IbgeCityModel
    {
        public int Id { get; set; }
        public string Nome { get; set; }
    }
}
