using DoAnWebNangCao.Data;
using DoAnWebNangCao.Data.Paging;
using DoAnWebNangCao.Models;
using DoAnWebNangCao.Repositories.Abstraction;
using Microsoft.EntityFrameworkCore;

namespace DoAnWebNangCao.Repositories.Implementation
{
    public class BookService : IBookService
    {
        private readonly ApplicationDbContext _context;
        public BookService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Add(Book book)
        {
            try
            {
                _context.Books.Add(book);
                await _context.SaveChangesAsync();
                foreach(int genreId in book.Genres)
                {
                    var bookGenre = new Genre_Book 
                    {
                        BookId = book.Id, 
                        GenreId = genreId,
                    };
                    _context.Genres_Book.Add(bookGenre);
                }
                await _context.SaveChangesAsync();
                foreach (int authorId in book.Authors)
                {
                    var bookAuthor = new Book_Author
                    {
                        BookId = book.Id,
                        AuthorId = authorId,
                    };
                    _context.Books_Author.Add(bookAuthor);
                }
               await _context.SaveChangesAsync();
               return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public async Task<bool> Delete(int id)
        {
            try
            {
                var data = await this.GetById(id);
                if (data == null)
                    return false;
                var bookGenres = _context.Genres_Book.Where(a => a.BookId == data.Id);
                var bookAuthors = _context.Books_Author.Where(a => a.BookId == data.Id);
                foreach (var bookGenre in bookGenres)
                {
                    _context.Genres_Book.Remove(bookGenre);
                }
                foreach (var bookAuthor in bookAuthors)
                {
                    _context.Books_Author.Remove(bookAuthor);
                }
                _context.Books.Remove(data);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

         public async Task<bool> Update(Book model)
        {
            try
            {
                var genresToDeleted = _context.Genres_Book.Where(a => a.BookId == model.Id && !model.Genres.Contains(a.GenreId)).ToList();
                foreach (var mGenre in genresToDeleted)
                {
                    _context.Genres_Book.Remove(mGenre);
                }
                foreach (int genId in model.Genres)
                {
                    var bookGenre = _context.Genres_Book.FirstOrDefault(a => a.BookId == model.Id && a.GenreId == genId);
                    if (bookGenre == null)
                    {
                        bookGenre = new Genre_Book { GenreId = genId, BookId = model.Id };
                        _context.Genres_Book.Add(bookGenre);
                    }
                }
                var authorsToDeleted = _context.Books_Author.Where(a => a.BookId == model.Id && !model.Authors.Contains(a.AuthorId)).ToList();
                foreach (var mAuthor in authorsToDeleted)
                {
                    _context.Books_Author.Remove(mAuthor);
                }
                foreach (int auId in model.Authors)
                {
                    var bookAuthor = _context.Books_Author.FirstOrDefault(a => a.BookId == model.Id && a.AuthorId == auId);
                    if (bookAuthor == null)
                    {
                        bookAuthor = new Book_Author { AuthorId = auId, BookId = model.Id };
                        _context.Books_Author.Add(bookAuthor);
                    }
                }
                _context.Books.Update(model);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public async Task<BookPaging> List(string searchString, string searchButton, string filterButton, string sortOrder, int? page)
        {

            var query =  _context.Books
             .Include(b => b.Publisher)
             .Include(d => d.Discount);
            switch (sortOrder)
            {
                case "BookName_desc":
                    query = query.OrderByDescending(b => b.BookName).Include(b => b.Publisher).Include(d => d.Discount);
                    break;
                case "Price":
                    query = query.OrderBy(b => b.Price).Include(b => b.Publisher).Include(d => d.Discount);
                    break;
                case "Price_desc":
                    query = query.OrderByDescending(b => b.Price).Include(b => b.Publisher).Include(d => d.Discount);
                    break;
                case "ReleaseYear":
                    query = query.OrderBy(b => b.ReleaseYear).Include(b => b.Publisher).Include(d => d.Discount);
                    break;
                case "ReleaseYear_desc":
                    query = query.OrderByDescending(b => b.ReleaseYear).Include(b => b.Publisher).Include(d => d.Discount);
                    break;
                default:
                    query = query.OrderBy(b => b.BookName).Include(b => b.Publisher).Include(d => d.Discount);
                    break;
            }

            foreach (var book in query)
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
            if (!string.IsNullOrEmpty(searchButton) && !String.IsNullOrEmpty(searchString))
            {
                query = query.Where(s => s.BookName.Contains(searchString)).Include(b => b.Publisher).Include(d => d.Discount);
            }
            int pageSize = 8;
            int pageNumber = page ?? 1;
            var bookPaging = new BookPaging
            {
                books = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync()
            };
            bookPaging.TotalPages = (int)Math.Ceiling(await query.CountAsync() / (double)pageSize);
            bookPaging.CurrentPage = pageNumber;
            return bookPaging;
        }

        public async Task<Book> GetById(int id)
        {
            return await _context.Books.FindAsync(id);
        }
        public List<int> GetGenreByBookId(int bookId)
        {
            var genreIds = _context.Genres_Book.Where(a => a.BookId == bookId).Select(a => a.GenreId).ToList();
            return genreIds;
        }
        public List<int> GetAuthorByBookId(int bookId)
        {
            var AuthorIds =  _context.Books_Author.Where(a => a.BookId == bookId).Select(a => a.AuthorId).ToList();
            return AuthorIds;
        }
    }
}
