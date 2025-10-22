using Microsoft.EntityFrameworkCore;
using SalesWebMVC.Data;
using SalesWebMVC.Models;
using SalesWebMVC.Models.ViewModels;
using System.Globalization;

namespace SalesWebMVC.Services
{
    public class StatsService
    {
        private readonly ApplicationDbContext _context;

        public StatsService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<StatsViewModel> GetSalesByMonthAsync()
        {
            var salesByMonth = await _context.SalesRecords
                .GroupBy(s => new { s.Date.Year, s.Date.Month })
                .Select(g => new
                {
                    Month = g.Key.Month,
                    Year = g.Key.Year,
                    Total = g.Sum(s => s.Amount)
                })
                .OrderBy(g => g.Year).ThenBy(g => g.Month)
                .ToListAsync();

            // Vendas por vendedor
            var salesBySeller = await _context.SalesRecords
                .Where(x => x.Status != SalesWebMVC.Models.Enums.SalesStatus.Canceled)
                .Include(s => s.Seller)
                .GroupBy(s => s.Seller.UserFullName)
                .Select(g => new {
                    Seller = g.Key,
                    Total = g.Sum(s => s.Amount)
                })
                .OrderByDescending(g => g.Total)
                .ToListAsync();

            return new StatsViewModel
            {
                Labels = salesByMonth.Select(x => $"{CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(x.Month)}/{x.Year}").ToList(),
                Sales = salesByMonth.Select(x => x.Total).ToList(),
                SellerNames = salesBySeller.Select(x => x.Seller).ToList(),
                SellerSales = salesBySeller.Select(x => x.Total).ToList()
            };
            
        }
    }
}




