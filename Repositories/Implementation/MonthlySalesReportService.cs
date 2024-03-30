using DoAnWebNangCao.Data;
using DoAnWebNangCao.Models;
using DoAnWebNangCao.Repositories.Abstraction;
using System.Globalization;

namespace DoAnWebNangCao.Repositories.Implementation
{
    public class MonthlySalesReportService : IMonthlySalesReportService
    {
        private readonly ApplicationDbContext _context;

        public MonthlySalesReportService(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<MonthlySalesReport> GetTotalSalesByMonth()
        {
            var result = _context.Order
                .Where(o => o.OrderStatusId == 4)
                .GroupBy(o => new { Year = o.CreatedDate.Year, Month = o.CreatedDate.Month })
                .Select(g => new MonthlySalesReport
                {
                    FirstDayOfMonth = new DateTime(g.Key.Year, g.Key.Month, 1),
                    LastDayOfMonth = new DateTime(g.Key.Year, g.Key.Month, DateTime.DaysInMonth(g.Key.Year, g.Key.Month)),
                    Month = $"{g.Key.Month}/{g.Key.Year}" // Format theo dạng "MM/YYYY"
                })
                .AsEnumerable() // Chuyển sang thực thi trên danh sách đã load vào bộ nhớ
                .Select(report =>
                {
                    report.TotalSales = _context.Order
                        .Where(o => o.OrderStatusId == 4 &&
                                    o.CreatedDate.Year == report.FirstDayOfMonth.Year &&
                                    o.CreatedDate.Month == report.FirstDayOfMonth.Month)
                        .Sum(o => o.TotalPrice);
                    return report;
                })
                .OrderBy(e => e.FirstDayOfMonth)
                .ToList();

            return result;
        }


    }
}
