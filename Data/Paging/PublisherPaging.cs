using DoAnWebNangCao.Models;

namespace DoAnWebNangCao.Data.Paging
{
    public class PublisherPaging
    {
        public List<Publisher> Publishers { get; set; }
        public string? SearchString { get; set; }
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
    }
}
