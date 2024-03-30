namespace DoAnWebNangCao.Models
{
    public class StockInput
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int BookId { get; set; }
        public int Quantity { get; set; }
        public double UnitPrice { get; set; }
        public Book Book { get; set; }
    }
}
