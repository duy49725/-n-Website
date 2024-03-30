using System.ComponentModel.DataAnnotations.Schema;

namespace DoAnWebNangCao.Models
{
    public class Publisher
    {
        public int Id { get; set; } 
        public string PublisherName { get; set; }
        public string PublisherAddress { get; set; }
        public List<Book> Books { get; set; }
    }
}
