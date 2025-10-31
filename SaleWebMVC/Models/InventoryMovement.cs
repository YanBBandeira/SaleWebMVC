using SalesWebMVC.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace SalesWebMVC.Models
{
    public class InventoryMovement
    {
        public int Id { get; set; }
        public int ProductId { get; set; } // Foreign key to Product
        public Product? Product { get; set; }

        [Required]
        [Display(Name = "Inventory status")]
        public MovementType MovementType { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be positive")]
        public int Quantity { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}")]
        public DateTime Date { get; set; } = DateTime.Now;

        public string? UserId { get; set; } // Foreign key to User
        public ApplicationUser? User { get; set; }

        [MaxLength(500)]
        public string? Observation { get; set; }
    }
}
