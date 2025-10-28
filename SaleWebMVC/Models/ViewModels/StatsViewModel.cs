using SalesWebMVC.Models.Enums;

namespace SalesWebMVC.Models.ViewModels
{
    public class StatsViewModel
    {
        public SalesByMonthViewModel SalesByMonth { get; set; }
        public SalesBySellerViewModel SalesBySeller { get; set; }
        public SalesByStatusViewModel SalesByStatus { get; set; }
        public SalesByDepartmentViewModel SalesByDepartment { get; set; }

        public StatsFilterViewModel Filter { get; set; }
    }

    public class SalesByStatusViewModel
    {
        public List<string> StatusLabel { get; set; }
        public Dictionary<string, List<double>> SalesPerStatus { get; set; } // Dictioney of sales per status

    }
    public class SalesByMonthViewModel
    {
        public List<string> MonthLabels { get; set; }
        public List<string> StatusLabel { get; set; }
        public Dictionary<string, List<double>> SalesPerStatus { get; set; } // Dictioney of sales per status

    }

    public class SalesBySellerViewModel
    {
        public List<string> SellerLabels { get; set; }
        public List<string> StatusLabel { get; set; }
        public Dictionary<string, List<double>> SalesPerStatus { get; set; } // Dictioney of sales per status
    }

    public class SalesByDepartmentViewModel
    {
        public List<string> DepartmentLabels { get; set; }
        public List<string> StatusLabel { get; set; }
        public Dictionary<string, List<double>> SalesPerStatus { get; set; } // Dictioney of sales per status
    }

    public class StatsFilterViewModel
    {
        public DateTime? minDate { get; set; }
        public DateTime? maxDate { get; set; }

        public List<int>? DepartmentId { get; set; }
        public List<string>? SellerId { get; set; }

        public SalesStatus? Status { get; set; }
    }
}
