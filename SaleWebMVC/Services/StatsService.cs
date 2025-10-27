using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NuGet.ProjectModel;
using SalesWebMVC.Data;
using SalesWebMVC.Models;
using SalesWebMVC.Models.Enums;
using SalesWebMVC.Models.ViewModels;
using System.Globalization;
using System.Linq;

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

            if (filter != null)
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


            // Agrupa vendas por mês e status e já ordena
            var SalesByMonthStatus = await salesQuery
                .GroupBy(s => new
                {
                    s.Date.Year,
                    s.Date.Month,
                    s.Status
                })
                .Select(s => new
                {
                    Year = s.Key.Year,
                    Month = s.Key.Month,
                    Status = s.Key.Status,
                    Total = s.Sum(s => s.Amount)
                })
                .OrderBy(g => g.Year)
                .ThenBy(g => g.Month)
                .ToListAsync();

            // Gera lista de labels únicas de mês no formato "MM/yyyy"
            var monthLabels = SalesByMonthStatus
                .Select(x => $"{x.Month:D2}/{x.Year}")  // Exemplo: "04/2025"
                .Distinct()
                .ToList();

            // Lista de status para exibir no gráfico
            var statusListByMonth = SalesByMonthStatus
                .Select(x => x.Status.ToString())
                .Distinct()
                .ToList();

            // Cria um lookup rápido: chave = (Status, MM/yyyy)
            var lookup = SalesByMonthStatus
                .ToDictionary(
                    x => (Status: x.Status.ToString(), Label: $"{x.Month:D2}/{x.Year}"),
                    x => x.Total
                );

            // Monta o dicionário de vendas por mês por status
            var salesPerStatusByMonth = statusListByMonth.ToDictionary(
                status => status,
                status => monthLabels
                    .Select(label => lookup.TryGetValue((status, label), out var total) ? total : 0.0)
                    .ToList()
            );







            // Seleciona a lista por vendedor
            var SalesBySellerStatus = await salesQuery
                .GroupBy(s => new
                {
                    s.Seller.UserFullName,
                    s.Status
                })
                .Select(g => new
                {
                    SellerName = g.Key.UserFullName,
                    Status = g.Key.Status,
                    Total = g.Sum(s => s.Amount)
                })
                .ToListAsync();

            var sellerLabels = SalesBySellerStatus
                .Select(x => x.SellerName)
                .Distinct()
                .ToList();


            // Pega os status distintos
            var statusListBySeller = SalesByMonthStatus
                .Select(x => x.Status.ToString())
                .Distinct()
                .ToList();

            // Monta o dicionário de vendas por mês por status
            var salesPerStatusBySeller = new Dictionary<string, List<double>>();

            foreach (var status in statusListBySeller)
            {
                var list = new List<double>();
                foreach (var seller in sellerLabels)
                {
                    var total = SalesBySellerStatus
                        .Where(x => x.Status.ToString() == status && x.SellerName == seller)
                        .Sum(x => x.Total);
                    list.Add(total);
                }
                salesPerStatusBySeller[status.ToString()] = list;
            }



            // Agrupa vendas por status e soma os valores
            var SalesByStatus = await salesQuery
                .GroupBy(s => s.Status)
                .Select(g => new
                {
                    Status = g.Key,
                    Total = g.Sum(s => s.Amount)
                })
                .ToListAsync();

            // Lista de status para exibir no gráfico
            var statusLabels = SalesByStatus
                .Select(x => x.Status)
                .Distinct()
                .ToList();


            // Monta o dicionário de vendas por status
            var salesPerStatus = SalesByStatus
                .ToDictionary(
                x => x.Status.ToString(),    // chave = nome do status
                x => new List<double> { x.Total } // valor = lista com 1 elemento
                );

            var SalesByDepartmentStatus = await salesQuery
                .GroupBy(s => new
                {
                    DepartmentName = s.Seller.Department.Name,
                    s.Status
                })
                .Select(g => new
                {
                    DepartmentName = g.Key.DepartmentName,
                    Status = g.Key.Status,
                    Total = g.Sum(s => s.Amount)
                })
                .ToListAsync();

            var statusLabelsByDepartment = SalesByDepartmentStatus
                .Select(x => x.Status.ToString())
                .Distinct()
                .ToList();

            var departmentLabels = SalesByDepartmentStatus
                .Select(x => x.DepartmentName)
                .Distinct()
                .ToList();

            //Monta o dicionário de vendas por mês por status
            var salesPerStatusByDepartment = new Dictionary<string, List<double>>();

            foreach (var status in statusLabelsByDepartment)
            {
                var list = new List<double>();
                foreach (var dept in departmentLabels)
                {
                    var total = SalesByDepartmentStatus
                        .Where(x => x.Status.ToString() == status && x.DepartmentName == dept)
                        .Sum(x => x.Total);
                    list.Add(total);
                }
                salesPerStatusByDepartment[status] = list;
            }

            var result = new StatsViewModel
            {
                SalesByMonth = new SalesByMonthViewModel
                {
                    MonthLabels = monthLabels,
                    StatusLabel = statusListByMonth,
                    SalesPerStatus = salesPerStatusByMonth
                },
                SalesBySeller = new SalesBySellerViewModel
                {
                    SellerLabels = sellerLabels,
                    StatusLabel = statusListBySeller,
                    SalesPerStatus = salesPerStatusBySeller
                },
                SalesByStatus = new SalesByStatusViewModel
                {
                    StatusLabel = statusLabels.Select(s => s.ToString()).ToList(),
                    SalesPerStatus = salesPerStatus
                },
                SalesByDepartment = new SalesByDepartmentViewModel
                {
                    DepartmentLabels = departmentLabels,
                    StatusLabel = statusLabelsByDepartment,
                    SalesPerStatus = salesPerStatusByDepartment
                }
            };
            return result;
        }
    }
}




