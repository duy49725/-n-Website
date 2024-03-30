using System.ComponentModel.DataAnnotations.Schema;

namespace DoAnWebNangCao.Models
{
    public class Author
    {
        public int Id { get; set; }
        public string? AuthorName { get; set; }
        public string? AuthorEmail { get; set; }
        public string? Image {  get; set; }
		[NotMapped]
		public IFormFile? ImageFile { get; set; }
		public string? nationality { get; set; }
    }
}
