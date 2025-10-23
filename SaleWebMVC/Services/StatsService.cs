using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NuGet.ProjectModel;
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

        public async Task<StatsViewModel> GetStatsAsync(StatsFilterViewModel? filter)
        {
            var salesQuery = _context.SalesRecords.AsQueryable();

            if(filter != null)
            {
                // Aplica os filtros
                if (filter.StartDate.HasValue)
                {
                    salesQuery = salesQuery.Where(s => s.Date >= filter.StartDate.Value);
                }
                if (filter.EndDate.HasValue)
                {
                    salesQuery = salesQuery.Where(s => s.Date <= filter.EndDate.Value);
                }
                if (filter.DepartmentId.HasValue)
                {
                    salesQuery = salesQuery.Where(s => s.DepartmentId == filter.DepartmentId.Value);
                }
                if (!string.IsNullOrEmpty(filter.SellerId))
                {
                    salesQuery = salesQuery.Where(s => s.SellerId == filter.SellerId);
                }
            }


            // seleciona a lista por mês
            var SalesByMonth = await salesQuery
                .GroupBy(s => new
                {
                    s.Date.Year,
                    s.Date.Month
                })
                .Select(s => new
                {
                    Year = s.Key.Year,
                    Month = s.Key.Month,
                    Total = s.Sum(s => s.Amount)
                })
                .ToListAsync();

       
            var monthLabels = SalesByMonth
                .Select(x => $"{x.Month:D2}/{x.Year}")  // Exemplo: "04/2025"
                .ToList();

            var monthSales = SalesByMonth
                .Select(x => x.Total)
                .ToList();


            // Seleciona a lista por vendedor
            var salesBySeller = await salesQuery
                .GroupBy(s => s.Seller.UserFullName)
                .Select(g => new
                {
                    SellerName = g.Key,
                    Total = g.Sum(s => s.Amount)
                })
                .OrderByDescending(g => g.Total)
                .ToListAsync();

            var sellerNames = salesBySeller.Select(x => x.SellerName).ToList();
            var sellerSales = salesBySeller.Select(x => x.Total).ToList();

            var result = new StatsViewModel
            {
                SalesByMonth = new SalesByMonthViewModel
                {
                    MonthLabels = monthLabels,
                    MonthSales = monthSales
                },
                SalesBySeller = new SalesBySellerViewModel
                {
                    SellerNames = sellerNames,
                    SellerSales = sellerSales
                }
            };
            return result;
        }
    }
}




