using DoAnWebNangCao.Data;
using DoAnWebNangCao.Models;
using DoAnWebNangCao.Repositories.Abstraction;
using DoAnWebNangCao.Repositories.Implementation;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Security.Policy;

namespace DoAnWebNangCao.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHomeService _homeService;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public HomeController(ILogger<HomeController> logger, IHomeService homeService, ApplicationDbContext context, UserManager<User> userManager)
        {
            _logger = logger;
            _homeService = homeService;
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(string sterm="")
        {
            IEnumerable<Book> books = await _homeService.GetAllBooks(sterm);
            return View(books);
        }

        public async Task<IActionResult> Detail(int id)
        {
            var query = _context.Books
             .Include(b => b.Publisher)
             .Include(R => R.Reviews);
               
            query = query.Where(s => s.Id == id).Include(b => b.Publisher).Include(R => R.Reviews);
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
            }
            var currentBook = await _context.Books
                            .Include(b => b.Publisher)
                            .FirstOrDefaultAsync(b => b.Id == id);

            if (currentBook == null)
            {
                return NotFound();
            }

            // Lấy danh sách thể loại của quyển sách hiện tại
            var genreIds = await _context.Genres_Book
                .Where(gb => gb.BookId == currentBook.Id)
                .Select(gb => gb.Genre.GenreName)
                .ToListAsync();
           
            // Truy vấn các quyển sách có chứa ít nhất một thể loại trùng khớp
            var relatedBooksQuery = _context.Books
                .Include(b => b.Publisher)
                .Where(b => b.Id != id && b.Genre_Book.Any(gb => genreIds.Contains(gb.Genre.GenreName)))
                .Select (b => new Book
                {
                    Id = b.Id,
                    BookName = b.BookName,
                    Image = b.Image,
                    Price = b.Price,

                });
            ViewBag.RelatedBooks = relatedBooksQuery;
            foreach (var review in currentBook.Reviews)
            {
                var user = await _userManager.FindByIdAsync(review.UserId);
                if (user != null)
                {
                    review.UserName = user.UserName;
                }
            }
            return View(query);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public async Task<IActionResult> ReviewPartial(BookReview review)
        {

                var user = await _userManager.GetUserAsync(User);
                review.UserId = user.Id;
                review.CreatedAt = DateTime.Now;

                _context.Add(review);
                await _context.SaveChangesAsync();

                var book = await _context.Books.FindAsync(review.BookId);
                book.AverageRating = _context.BookReviews.Where(r => r.BookId == review.BookId).Average(r => r.Rating);
                await _context.SaveChangesAsync();
                var userName = await _userManager.FindByIdAsync(review.UserId);
                return RedirectToAction("Detail", "Home", new { Id = review.BookId });
            

            return View(review);
        }
        public IActionResult Filter(ProductFilterViewModel filter)
        {
            // Load Filter Options
            filter.Authors = _context.Authors
                .Select(a => new AuthorCheckboxViewModel
                {
                    Id = a.Id,
                    AuthorName = a.AuthorName,
                    IsSelected = filter.SelectedAuthorIds != null && filter.SelectedAuthorIds.Contains(a.Id)
                })
                .ToList();

            filter.Genres = _context.Genres
                .Select(g => new GenreCheckboxViewModel
                {
                    Id = g.Id,
                    GenreName = g.GenreName,
                    IsSelected = filter.SelectedGenreIds != null && filter.SelectedGenreIds.Contains(g.Id)
                })
                .ToList();

            // Apply Filters
            IQueryable<Book> booksQuery = _context.Books
                .Include(b => b.Book_Author).ThenInclude(gb => gb.Authors) // Include để lấy thông tin tác giả
                .Include(b => b.Genre_Book).ThenInclude(gb => gb.Genre); // Include để lấy thông tin thể loại

            if (filter.SelectedAuthorIds != null && filter.SelectedAuthorIds.Any())
            {
                booksQuery = booksQuery.Where(b => b.Book_Author.Any(a => filter.SelectedAuthorIds.Contains(a.AuthorId)));
            }

            if (filter.SelectedGenreIds != null && filter.SelectedGenreIds.Any())
            {
                booksQuery = booksQuery.Where(b => b.Genre_Book.Any(gb => filter.SelectedGenreIds.Contains(gb.GenreId)));
            }

            // Sort by Price
            switch (filter.PriceSort)
            {
                case "asc":
                    booksQuery = booksQuery.OrderBy(b => b.Price);
                    break;
                case "desc":
                    booksQuery = booksQuery.OrderByDescending(b => b.Price);
                    break;
                default:
                    // Default sorting (you can change this to your requirement)
                    booksQuery = booksQuery.OrderBy(b => b.Price);
                    break;
            }

            // Get Filtered Books
            var filteredBooks = booksQuery.ToList();

            // Pass the ViewModel to the View
            var viewModel = new ProductIndexViewModel
            {
                Filter = filter,
                Books = filteredBooks
            };

            return View(viewModel);
        }

        public async Task<IActionResult> DiscountedBooks()
        {
            // Lấy danh sách các quyển sách có giảm giá
            var discountedBooks = await _homeService.GetAllBooks();

            return View(discountedBooks);
        }
    }
}