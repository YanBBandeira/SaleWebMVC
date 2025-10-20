using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using SalesWebMVC.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace SalesWebMVC.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Display(Name = "Name")]
        public string UserFullName { get; set; }
        
        [DataType(DataType.Date)]
        [Display(Name = "Birth Date")]
        public DateTime? BirthDate { get; set; }

        
        [Display(Name = "Base Salary")]
        [Range(1.0,5000000.0, ErrorMessage = "{0} must be from {1} to {2}")]
        public double? BaseSalary { get; set; }
        [Display(Name = "Department")]
        public int? DepartmentId { get; set; }

        [ValidateNever]
        public Department? Department{ get; set; }

        public ICollection<SalesRecord>? SalesRecords { get; set; } = new List<SalesRecord>();

        public SellerStatus? SellerStatus { get; set; } = Enums.SellerStatus.Active;

        // Construtores
        public ApplicationUser() { }

        public ApplicationUser(string userName, string email, string fullName, DateTime birthDate, double baseSalary, int departmentId)
        {
            UserName = userName;
            Email = email;
            UserFullName = fullName;
            BirthDate = birthDate;
            BaseSalary = baseSalary;
            DepartmentId = departmentId;
            EmailConfirmed = true;
        }

        // Método para total de vendas no período
        public double TotalSales(DateTime initial, DateTime final)
        {
            return SalesRecords
                .Where(sr => sr.Date >= initial && sr.Date <= final)
                .Sum(sr => sr.Amount);
        }


    }

}

