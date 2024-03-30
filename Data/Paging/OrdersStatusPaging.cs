using DoAnWebNangCao.Models;

namespace DoAnWebNangCao.Data.Paging
{
    public class OrdersStatusPaging
    {
        public List<OrderStatus>? OrderStatuses { get; set; }
        public string? SearchString { get; set; }
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
    }
}
