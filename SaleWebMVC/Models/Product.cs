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
        [Required]
        public int StockQuantity { get; set; } //Atributo crucial

        [Display(Name = "Current stock")]
        [Required]
        public int MinimumStock { get; set; } // Novo atributo para alertas

        [Display(Name = "Supplier")]
        public int SupplierId { get; set; } // Chave estrangeira para o fornecedor
        public Supplier Supplier { get; set; } // Relação com o fornecedor

        [Display(Name = "Department")]
        public int DepartmentId { get; set; } // Chave estrangeira para o departamento
        public Department Department { get; set; } // Relação com o departamento

        public bool NeedsRestock()
        {
            return StockQuantity <= MinimumStock;
        }

        public ICollection<InventoryMovement> InventoryMovements { get; set; } = new List<InventoryMovement>(); 
        // Relação com os movimentos de inventário
    }
}
