namespace DoAnWebNangCao.Models
{
    public class Genre_Book
    {

        public int Id { get; set; }
        public int BookId { get; set; }
        public int GenreId { get; set; }
        public Genre Genre { get; set; }
    }
}
