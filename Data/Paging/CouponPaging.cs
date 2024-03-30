using DoAnWebNangCao.Models;

namespace DoAnWebNangCao.Data.Paging
{
    public class CouponPaging
    {
        public List<Coupon> Coupons { get; set; }
        public string? SearchString { get; set; }
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
    }
}
