using DoAnWebNangCao.Models;

namespace DoAnWebNangCao.Data.Paging
{
    public class BookPaging
    {
        public List<Book>? books { get; set; }
        public string? SearchString { get; set; }
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
    }
}
