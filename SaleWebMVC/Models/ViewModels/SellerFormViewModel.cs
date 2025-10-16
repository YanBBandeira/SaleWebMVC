using System.Collections.Generic;

namespace SalesWebMVC.Models.ViewModels
{
    public class SellerFormViewModel
    {
        public ICollection<Department> Departments { get; set; }

        public ApplicationUser Seller { get; set; }
    }
}
