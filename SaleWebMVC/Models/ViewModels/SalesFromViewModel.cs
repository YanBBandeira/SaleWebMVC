using SalesWebMVC.Models.Enums;

namespace SalesWebMVC.Models.ViewModels
{
    public class SalesFromViewModel
    {
        public SalesStatus Status { get; set; }
        public SalesRecord SalesRecord { get; set; }

        // A LISTA de vendedores disponíveis para o dropdown
        public ICollection<Department> Departments { get; set; }
        public ICollection<ApplicationUser> Sellers { get; set; }

        
        // A coleção de OrderItems que será preenchida pelo formulário.
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
        public ICollection<ProductDto> Products { get; set; } = new List<ProductDto>();

        // =======================================================
        // NOVO: DTO Aninhado (ViewModel de Produto)
        // =======================================================
        public class ProductDto
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public double Price { get; set; }

            // Nenhum relacionamento de navegação de EF Core aqui!
        }
    }
}
