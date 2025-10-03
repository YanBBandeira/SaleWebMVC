using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace SalesWebMVC.Models
{
    public class Department
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "{0} required")]
        [StringLength(60, MinimumLength = 2, ErrorMessage = "{0} size should be between {1} and {2}")]
        public string Name { get; set; }
        public ICollection<Seller> Sellers { get; set; } = new List<Seller>();

        public Department() { }
        public Department(int id, string name)
        {
            Id = id;
            Name = name;
        }


        public void AddSeller(Seller seller) 
        {
            Sellers.Add(seller);
        }

        public double TotalSales(DateTime initialDate, DateTime endDate)
        {
            return (from p in Sellers select p.TotalSales(initialDate, endDate)).Sum();
        }

    }
}
