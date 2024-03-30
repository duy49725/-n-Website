using DoAnWebNangCao.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DoAnWebNangCao.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
            : base(options) 
        {

        }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Author> Authors{ get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Publisher> Publishers { get; set; }
        public DbSet<Genre_Book> Genres_Book { get; set; }
        public DbSet<Book_Author> Books_Author { get; set; }
        public DbSet<Cart> Carts { get; set; }  
        public DbSet<CartDetail> CartDetails { get; set;}
        public DbSet<Order> Order { get; set; } 
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<OrderStatus> OrderStatus { get; set; }
        public DbSet<RevenueReport> RevenueReports { get; set;}
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<BookReview> BookReviews { get; set; }
        public DbSet<Discount> Discounts { get; set; }
        public DbSet<StockInput> StockInputs { get; set; }
        public DbSet<Coupon> Coupons { get; set; }
    
    }


}
