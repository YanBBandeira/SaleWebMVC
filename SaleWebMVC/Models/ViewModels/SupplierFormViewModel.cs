namespace SalesWebMVC.Models.ViewModels
{
    public class SupplierFormViewModel
    {
        public Supplier Supplier { get; set; }
        public ICollection<State> States { get; set; }
        public ICollection<City> Cities { get; set; }
    }
}
