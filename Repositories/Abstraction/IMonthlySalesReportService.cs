using DoAnWebNangCao.Data;
using DoAnWebNangCao.Models;

namespace DoAnWebNangCao.Repositories.Abstraction
{
    public interface IMonthlySalesReportService
    {
        List<MonthlySalesReport> GetTotalSalesByMonth();
    }
}
