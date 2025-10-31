namespace SalesWebMVC.Models.ViewModels
{
    public class ProductFormViewModel
    {
        public Product Product { get; set; }
        public ICollection<Supplier> Suppliers { get; set; }
        public ICollection<Department> Departments { get; set; }
    }
}
