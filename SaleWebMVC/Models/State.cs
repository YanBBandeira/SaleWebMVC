using System.ComponentModel.DataAnnotations;

namespace SalesWebMVC.Models
{
    public class State
    {
        public int Id { get; set; } // Código único do estado ou código do IBGE
        [Display(Name = "State")]
        public string Name { get; set; }
        public string UF { get; set; } //Sigla do estado

        public ICollection<City> Cities { get; set; } = new List<City>(); // Relação com as cidades do estado
    }
}
