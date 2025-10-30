using System.ComponentModel.DataAnnotations;

namespace SalesWebMVC.Models
{
    public class Supplier
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Display(Name = "CNPJ/CPF")]
        public string CNPJ { get; set; }

        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }
        [Phone(ErrorMessage = "Invalid Phone Number")]
        public string Phone { get; set; }

        public int CityId { get; set; } // Chave estrangeira para a cidade
        public City City { get; set; } // Relação com a cidade

        public ICollection<Product> Products { get; set; } = new List<Product>(); // Relação com os produtos fornecidos
    }
}
