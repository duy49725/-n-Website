using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace DoAnWebNangCao.Models
{
    public class BookReview
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        [Required]
        public int BookId { get; set; }

        [Required]
        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5")]
        public int Rating { get; set; }

        public string Comment { get; set; }

        public DateTime CreatedAt { get; set; }

        public User User { get; set; }

        public Book Book { get; set; }
        [NotMapped]
        public string UserName { get; set; }
    }
}
