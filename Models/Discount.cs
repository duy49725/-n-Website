namespace DoAnWebNangCao.Models
{
    public class Discount
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double DiscountPercentage { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }

        public List<Book> Books { get; set; }
    }
}
