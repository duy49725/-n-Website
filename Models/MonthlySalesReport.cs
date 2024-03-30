namespace DoAnWebNangCao.Models
{
    public class MonthlySalesReport
    {
        public string Month { get; set; }
        public DateTime FirstDayOfMonth { get; set; } // Ngày đầu tháng
        public DateTime LastDayOfMonth { get; set; } // Ngày cuối tháng
        public double TotalSales { get; set; }
    }

}
