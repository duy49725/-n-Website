namespace DoAnWebNangCao.Models
{
    public class Book_Author
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public int AuthorId { get; set; }    
        public Author Authors { get; set; }
    }
}
