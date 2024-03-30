using DoAnWebNangCao.Data;
using DoAnWebNangCao.Models;
using DoAnWebNangCao.Repositories.Abstraction;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace DoAnWebNangCao.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IMonthlySalesReportService _monthlySalesReportService;

        public DashboardController(ApplicationDbContext context, IMonthlySalesReportService monthlySalesReportService)
        {
            _context = context;
            _monthlySalesReportService = monthlySalesReportService;
        }

        public IActionResult MonthlySalesReport()
        {
            // Lấy dữ liệu từ service hoặc repository
            var monthlySales = _monthlySalesReportService.GetTotalSalesByMonth();

            // Tạo list các tháng và tổng doanh số tương ứng
            var monthLabels = monthlySales.Select(s => s.Month).ToList();
            var totalSales = monthlySales.Select(s => s.TotalSales).ToList();

            var model = new SalesChartViewModel
            {
                MonthLabels = monthLabels,
                TotalSales = totalSales
            };

            return View(model);
        }

        public IActionResult RevenueReport()
        {
            var pendingOrders = _context.Order
                .Where(o => o.OrderStatus.StatusName == "Pending")
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Book)
                .ToList();
            var todayOrders = pendingOrders.Where(o => o.CreatedDate.Date == DateTime.Today);
            var model = new DashboardViewModel
            {
                TodayRevenue = todayOrders
                    .GroupBy(o => o.CreatedDate.Date)
                    .Select(g => new RevenueReport
                    {
                        Date = g.Key,
                        TotalRevenue = g.Sum(o => o.TotalPrice)
                    })
                    .ToList(),
                DailyRevenue = pendingOrders
                    .GroupBy(o => o.CreatedDate.Date)
                    .Select(g => new RevenueReport
                    {
                        Date = g.Key,
                        TotalRevenue = g.Sum(o => o.TotalPrice)
                    })
                    .ToList(),

                MonthlyRevenue = pendingOrders
                    .GroupBy(o => new { Year = o.CreatedDate.Year, Month = o.CreatedDate.Month })
                    .Select(g => new RevenueReport
                    {
                        Date = new DateTime(g.Key.Year, g.Key.Month, 1),
                        TotalRevenue = g.Sum(o => o.TotalPrice)
                    })
                    .ToList(),

                YearlyRevenue = pendingOrders
                    .GroupBy(o => o.CreatedDate.Year)
                    .Select(g => new RevenueReport
                    {
                        Date = new DateTime(g.Key, 1, 1),
                        TotalRevenue = g.Sum(o => o.TotalPrice)
                    })
                    .ToList(),
                ProductRevenues = pendingOrders
                    .SelectMany(o => o.OrderDetails)
                    .GroupBy(od => od.BookId)
                    .Select(g => new ProductRevenue
                    {
                        ProductName = g.First().Book.BookName,
                        TotalRevenue = g.Sum(od => od.Quantity * od.UnitPrice),
                        SoldQuantity = g.Sum(od => od.Quantity)
                    })
                    .ToList(),
                TotalProduct = _context.Books.Sum(b => b.StockQuantity),
                TotalOrdersByStatus = _context.Order
			        .GroupBy(o => o.OrderStatusId)
			        .Select(g => new OrderStatusReport
			        {
				        OrderStatusId = g.Key,
				        StatusName = g.FirstOrDefault().OrderStatus.StatusName,
				        TotalOrders = g.Count()
			        })
			        .ToList(),
                TotalOrderbyDay = _context.Order
                     .Count(o => o.OrderStatusId == 4 &&
                        o.CreatedDate.Date == DateTime.Today)
            };

            return View(model);
        }


    }

}
