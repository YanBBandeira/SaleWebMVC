using System.ComponentModel.DataAnnotations;

namespace SalesWebMVC.Models
{
    public class City
    {
        public int Id { get; set; } // Código único da cidade ou código do IBGE
        [Display(Name = "City")]
        public string Name { get; set; }
        public int StateId { get; set; } // Chave estrangeira para o estado
        public State State { get; set; } // Relação com o estado

        public ICollection<SalesRecord> SaleRecords { get; set; } = new List<SalesRecord>(); // Relação com os registros de venda na cidade
        public ICollection<Supplier> Suppliers { get; set; } = new List<Supplier>(); // Relação com os fornecedores na cidade
    }
}
