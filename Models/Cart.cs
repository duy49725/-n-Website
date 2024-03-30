namespace DoAnWebNangCao.Models
{
    public class Cart
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public bool IsDeleted { get; set; } = false;
        public int? CouponId { get; set; } 
        public Coupon Coupon { get; set; }
        public ICollection<CartDetail> CartDetails { get; set; }
    }
}
