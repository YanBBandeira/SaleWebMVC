using SalesWebMVC.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace SalesWebMVC.Models
{
    public class SalesRecord
    {
        public int Id { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime Date { get; set; }

        [DisplayFormat(DataFormatString = "{0:F2}")] //0 representa o valor do atributo
        public double Amount { get; set; }
        public SalesStatus Status { get; set; }
        public Seller Seller { get; set; }

        [Display(Name = "Seller")]
        public int SellerId { get; set; }

        [Display(Name = "Department")]
        public int DepartmentId { get; set; }

        public SalesRecord() 
        { 
        }

        public SalesRecord(int id, DateTime date, double amount, SalesStatus status, Seller seller)
        {
            Id = id;
            Date = date;
            Amount = amount;
            Status = status;
            Seller = seller;
        }

    }
}
