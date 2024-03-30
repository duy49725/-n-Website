using DoAnWebNangCao.Models;

namespace DoAnWebNangCao.Data.Paging
{
    public class DiscountPaging
    {
        public List<Discount> Discounts { get; set; }
        public string? SearchString { get; set; }
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
    }
}
