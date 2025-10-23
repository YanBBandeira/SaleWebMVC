namespace SalesWebMVC.Models.ViewModels
{
    public class StatsViewModel
    {

        public SalesByMonthViewModel SalesByMonth { get; set; }
        public SalesBySellerViewModel SalesBySeller { get; set; }

        public StatsFilterViewModel Filter { get; set; }


    }

    public class SalesByMonthViewModel
    {
        public List<string> MonthLabels { get; set; }
        public List<double> MonthSales { get; set; }
    }

    public class SalesBySellerViewModel
    {
        public List<string> SellerNames { get; set; }
        public List<double> SellerSales { get; set; }
    }

    public class StatsFilterViewModel
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public int? DepartmentId { get; set; }
        public int? SellerId { get; set; }

        // Adicione mais filtros conforme necessidade
    }
}
