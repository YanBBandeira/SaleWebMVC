namespace SalesWebMVC.Models.ViewModels
{
    public class StatsViewModel
    {
        public List<string> Labels { get; set; } // Ex: ["Jan", "Feb", "Mar"]
        public List<double> Sales { get; set; } // Ex: [1000, 2300, 1800]

        // NOVO: Vendas por vendedor
        public List<string> SellerNames { get; set; }
        public List<double> SellerSales { get; set; }
    }
}
