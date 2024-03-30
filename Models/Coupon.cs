namespace DoAnWebNangCao.Models
{
    public class Coupon
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public double DiscountPercentage { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }
    }
}
