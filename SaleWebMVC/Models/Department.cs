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
        [Display(Name="Department")]
        public string Name { get; set; }
        public ICollection<ApplicationUser> Sellers { get; set; } = new List<ApplicationUser>();

        public Department() { }
        public Department(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public double TotalSales(DateTime initialDate, DateTime endDate)
        {
            return Sellers
                .SelectMany(s => s.SalesRecords) // acessa as vendas do usuário
                .Where(sr => sr.Date >= initialDate && sr.Date <= endDate)
                .Sum(sr => sr.Amount);
        }
    }
}
