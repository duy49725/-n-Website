using DoAnWebNangCao.Models;

namespace DoAnWebNangCao.Data.Paging
{
    public class GenrePaging
    {
        public List<Genre>? genres { get; set; }
        public string? SearchString { get; set; }
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
    }
}
