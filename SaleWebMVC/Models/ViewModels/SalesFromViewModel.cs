using SalesWebMVC.Models.Enums;

namespace SalesWebMVC.Models.ViewModels
{
    public class SalesFromViewModel
    {
        public SalesStatus Status { get; set; }
        public SalesRecord SalesRecord { get; set; }

        // A LISTA de vendedores disponíveis para o dropdown
        public ICollection<Seller> Sellers { get; set; }

        // Lista de departamentos

        public ICollection<Department> Departments { get; set; }
        public IEnumerable<SalesWebMVC.Models.SalesRecord> Sales { get; set; } 
    }
}
