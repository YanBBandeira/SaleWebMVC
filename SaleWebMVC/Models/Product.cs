using System.ComponentModel.DataAnnotations;

namespace SalesWebMVC.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome do produto é obrigatório")]
        public string Name { get; set; }

        [Display(Name = "Price")]
        [DataType(DataType.Currency)]
        public double Price { get; set; }

        [Display(Name = "Stock Quantity")]
        public int StockQuantity { get; set; }

        [Display(Name = "Supplier")]
        public int SupplierId { get; set; } // Chave estrangeira para o fornecedor
        public Supplier Supplier { get; set; } // Relação com o fornecedor
        public int CityId { get; set; } // Chave estrangeira para a cidade
        public City City { get; set; } // Relação com a cidade

        [Display(Name = "Department")]
        public int DepartmentId { get; set; } // Chave estrangeira para o departamento
        public Department Department { get; set; } // Relação com o departamento
    }
}
