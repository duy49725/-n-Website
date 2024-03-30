using System.ComponentModel.DataAnnotations.Schema;

namespace DoAnWebNangCao.Models
{
    public class Blog
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Link { get; set; }
        public string ImagePath { get; set; }
 
    }
}
