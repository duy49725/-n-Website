using DoAnWebNangCao.Models;

namespace DoAnWebNangCao.Data.Paging
{
    public class AuthorPaging
    {
        public List<Author>? author { get; set; }
        public string? SearchString { get; set; }
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
    }
}
