using DoAnWebNangCao.Models;

namespace DoAnWebNangCao.Data.Paging
{
    public class StockInputPaging
    {
        public List<StockInput> StockInputs { get; set; }
        public string? SearchString { get; set; }
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
    }
}
