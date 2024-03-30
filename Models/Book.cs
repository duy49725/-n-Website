using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DoAnWebNangCao.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string BookName { get; set; }
        public string Image { get; set; }
        public double Price { get; set; }
        public string Describle { get; set; }
        public string ReleaseYear { get; set; }
        public int PublisherId { get; set; }
        public double AverageRating { get; set; }
        public int? DiscountId { get; set; }
        public int StockQuantity { get; set; }
        public Discount Discount { get; set; }
        public Publisher Publisher { get; set; }
        public ICollection<Genre_Book> Genre_Book { get; set; }
        public ICollection<Book_Author> Book_Author { get; set; }
        public List<BookReview> Reviews { get; set; }
        [NotMapped]
        public IEnumerable<SelectListItem>? PublisherList { get; set; }
        [NotMapped]
        public IEnumerable<SelectListItem>? DiscountList { get; set; }
        [NotMapped]
        public IFormFile? ImageFile { get; set; }
        [NotMapped]
        [Required]
        public List<int>? Authors { get; set; }
        [NotMapped]
        [Required]
        public List<int>? Genres { get; set; }
        [NotMapped]
        public IEnumerable<SelectListItem>? GenreList { get; set; }
        [NotMapped]
        public string? GenreNames { get; set; }
        [NotMapped]
        public MultiSelectList? MultiGenreList { get; set; }
        [NotMapped]
        public IEnumerable<SelectListItem>? AuthorList { get; set; }
        [NotMapped]
        public string? AuthorNames { get; set; }
        [NotMapped]
        public MultiSelectList? MultiAuthorList { get; set; }
        [NotMapped]
        public string? PublisherName { get; set; }
        [NotMapped]
        public ICollection<Author> Author { get; set; }
        [NotMapped]
        public double? DiscountPercentage { get; set; }
    }
 }
