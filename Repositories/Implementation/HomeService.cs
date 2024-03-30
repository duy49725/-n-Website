using DoAnWebNangCao.Data;
using DoAnWebNangCao.Models;
using DoAnWebNangCao.Repositories.Abstraction;
using Microsoft.EntityFrameworkCore;

namespace DoAnWebNangCao.Repositories.Implementation
{
    public class HomeService : IHomeService
    {
        private readonly ApplicationDbContext _context;
        public HomeService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Genre>> Genres()
        {
            return await _context.Genres.ToListAsync();
        }

        public async Task<IEnumerable<Book>> GetAllBooks(string sTerm="")
        {
            sTerm = sTerm.ToLower();
            IEnumerable<Book> books = await _context.Books
                                  .Include(b => b.Publisher)
                                  .Include(b => b.Discount).ToListAsync();
            foreach (var book in books)
            {
                // Lấy danh sách thể loại của sách
                var genres = await _context.Genres_Book
                    .Where(gb => gb.BookId == book.Id)
                    .Select(gb => gb.Genre.GenreName)
                    .ToListAsync();

                // Lấy danh sách tác giả của sách
                var authors = await _context.Books_Author
                    .Where(ba => ba.BookId == book.Id)
                    .Select(ba => ba.Authors.AuthorName)
                    .ToListAsync();

                // Gán danh sách thể loại và tác giả vào các thuộc tính của sách
                book.GenreNames = string.Join(", ", genres);
                book.AuthorNames = string.Join(", ", authors);
                book.PublisherName = book.Publisher?.PublisherName;
                book.DiscountPercentage = book.Discount?.DiscountPercentage;
            }
            //return View(query);
            // Lấy danh sách các quyển sách có giảm giá từ cơ sở dữ liệu
            books = await (from book in _context.Books
                                    where string.IsNullOrWhiteSpace(sTerm) || (book != null && book.BookName.ToLower().StartsWith(sTerm))
                                    select book
                            ).ToListAsync();

            return books;
        }

        public async Task<Book> GetById(int id)
        {
            return await _context.Books.FindAsync(id);
        }

        public async Task<IEnumerable<Book>> GetDiscountedBooks()
        {
            var discountedBooks = await _context.Books
                                    .Include(b => b.Publisher)
                                    .Include(b => b.Discount).ToListAsync();
            foreach (var book in discountedBooks)
            {
                // Lấy danh sách thể loại của sách
                var genres = await _context.Genres_Book
                    .Where(gb => gb.BookId == book.Id)
                    .Select(gb => gb.Genre.GenreName)
                    .ToListAsync();

                // Lấy danh sách tác giả của sách
                var authors = await _context.Books_Author
                    .Where(ba => ba.BookId == book.Id)
                    .Select(ba => ba.Authors.AuthorName)
                    .ToListAsync();

                // Gán danh sách thể loại và tác giả vào các thuộc tính của sách
                book.GenreNames = string.Join(", ", genres);
                book.AuthorNames = string.Join(", ", authors);
                book.PublisherName = book.Publisher?.PublisherName;
                book.DiscountPercentage = book.Discount?.DiscountPercentage;
            }
            //return View(query);
            // Lấy danh sách các quyển sách có giảm giá từ cơ sở dữ liệu
             discountedBooks = await _context.Books
                .Include(b => b.Publisher)
                .Include(b => b.Discount)
                .Where(b => b.Discount != null && b.Discount.IsActive)
                .ToListAsync();

            return discountedBooks;
        }
    }
}
