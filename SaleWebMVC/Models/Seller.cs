using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
namespace SalesWebMVC.Models
{
    public class Seller
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "{0} required")]
        [StringLength(60, MinimumLength = 2, ErrorMessage = "{0} size should be between {1} and {2}")]
        // {0} pega o valor do atributo, {1} pega o valor da primeira inserção acima e {2} pega o valor da segunda
        public string Name { get; set; }

        [Required(ErrorMessage = "{0} required")]
        [EmailAddress(ErrorMessage = "Entre a valid email")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "E-mail")]
        public string Email { get; set; }


        [Required(ErrorMessage = "{0} required")]
        [Display(Name = "Birth Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")] //0 representa o valor do atributo
        public DateTime BirthDate { get; set; }

        [Display(Name = "Base Salary")]
        [Required(ErrorMessage = "{0} required")]
        [Range(100.0, 50000.0, ErrorMessage = "{0} must be from {1} to {2}")]
        [DisplayFormat(DataFormatString = "{0:F2}")] //0 representa o valor do atributo
        public double BaseSalary { get; set; }

        public Department Department { get; set; }
        [Display(Name = "Department")]
        public int DepartmentId { get; set; }

        public ICollection<SalesRecord> Sales { get; set; } = new List<SalesRecord>();

        public Seller()
        {

        }
        public Seller(int id, string name, string email, DateTime birthDate, double baseSalary, Department department)
        {
            Id = id;
            Name = name;
            Email = email;
            BirthDate = birthDate;
            BaseSalary = baseSalary;
            Department = department;
        }

        public void AddSales(SalesRecord sr)
        {
            Sales.Add(sr);
        }

        public void RemoveSales(SalesRecord sr)
        {
            Sales.Remove(sr);
        }

        public double TotalSales(DateTime initialDate, DateTime endDate)
        {
            return (from p in Sales
                    where p.Date >= initialDate && p.Date <= endDate
                    select p.Amount).Sum();
        }
    }
}
