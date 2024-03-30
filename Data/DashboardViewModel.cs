using DoAnWebNangCao.Models;

namespace DoAnWebNangCao.Data
{
    public class DashboardViewModel
    {
        public List<RevenueReport> TodayRevenue { get; set; }
        public List<RevenueReport> DailyRevenue { get; set; }
        public List<RevenueReport> MonthlyRevenue { get; set; }
        public List<RevenueReport> YearlyRevenue { get; set; }
        public List<ProductRevenue> ProductRevenues { get; set; }
        public double TotalProduct {  get; set; }
        public List<OrderStatusReport> TotalOrdersByStatus { get; set; }
        public int TotalOrderbyDay { get; set; }
    }

    public class ProductRevenue
    {
        public string ProductName { get; set; }
        public double TotalRevenue { get; set; }
        public int SoldQuantity { get; set; } 
    }

	public class OrderStatusReport
	{
		public int OrderStatusId { get; set; }
		public string StatusName { get; set; }
		public int TotalOrders { get; set; }
	}
}
