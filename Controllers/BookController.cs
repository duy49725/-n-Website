using DoAnWebNangCao.Data;
using DoAnWebNangCao.Models;
using DoAnWebNangCao.Repositories.Abstraction;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NuGet.Versioning;

namespace DoAnWebNangCao.Controllers
{
    public class BookController : Controller
    {
        private readonly IBookService _bookService;
        private readonly IFileService _fileService;
        private readonly IGenreService _genreService;
        private readonly IAuthorService _auhorService;
        private readonly IPublisherService _publisherService;
        private readonly IDiscountService _discountService;
        private readonly ApplicationDbContext _context;
        public BookController(IBookService bookService, IFileService fileService, IGenreService genreService, IAuthorService auhorService, ApplicationDbContext context, IPublisherService publisherService, IDiscountService discountService)
        {
            _context = context;
            _bookService = bookService;
            _fileService = fileService;
            _genreService = genreService;
            _auhorService = auhorService;
            _publisherService = publisherService;
            _discountService = discountService;
        }

        public async Task<IActionResult> Add()
        {
            var model = new Book();
            model.GenreList = _genreService.List().Select(a => new SelectListItem { Text = a.GenreName, Value = a.Id.ToString() });
            model.AuthorList = _auhorService.List().Select(a => new SelectListItem { Text = a.AuthorName, Value = a.Id.ToString() });
            model.PublisherList = _publisherService.List().Select(a => new SelectListItem { Text = a.PublisherName, Value = a.Id.ToString() });
            model.DiscountList = _discountService.List().Select(a => new SelectListItem { Text = a.DiscountPercentage.ToString(), Value = a.Id.ToString() });
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Add(Book model)
        {
           
            model.GenreList = _genreService.List().Select(a => new SelectListItem { Text = a.GenreName, Value = a.Id.ToString() });
            model.AuthorList = _auhorService.List().Select(a => new SelectListItem { Text = a.AuthorName, Value = a.Id.ToString() });
            model.PublisherList = _publisherService.List().Select(a => new SelectListItem { Text = a.PublisherName, Value = a.Id.ToString() });
            model.DiscountList = _discountService.List().Select(a => new SelectListItem { Text = a.DiscountPercentage.ToString(), Value = a.Id.ToString() });


            if (model.ImageFile != null)
            {
                var fileResult = this._fileService.SaveImage(model.ImageFile);
                if (fileResult.Item1 == 0)
                {
                    TempData["msg"] = "File could not saved";
                    return View(model);
                }
                var imageName = fileResult.Item2;
                model.Image = imageName;
            }
            var result = await _bookService.Add(model);
            if (result)
            {
                TempData["msg"] = "Added Successfully";
                return RedirectToAction(nameof(Add));
            }
            else
            {
                TempData["msg"] = "Error on server side";
                return View(model);
            }
        }

        public async Task<IActionResult> Edit(int id)
        {
            var data = await _bookService.GetById(id);
            data.PublisherList = _publisherService.List().Select(a => new SelectListItem { Text = a.PublisherName, Value = a.Id.ToString() });
            data.DiscountList = _discountService.List().Select(a => new SelectListItem { Text = a.DiscountPercentage.ToString(), Value = a.Id.ToString() });
            var selectedGenres = _bookService.GetGenreByBookId(id);
            MultiSelectList selectListItemsGenre = new MultiSelectList(_genreService.List(), "Id", "GenreName", selectedGenres);
            data.MultiGenreList = selectListItemsGenre;
            var selectedAuthor = _bookService.GetAuthorByBookId(id);
            MultiSelectList selectListItemsAuthor = new MultiSelectList(_auhorService.List(), "Id", "AuthorName", selectedAuthor);
            data.MultiAuthorList = selectListItemsAuthor;
            return View(data);
        }
        public async Task<IActionResult> Detail(int id)
        {
            var query = _context.Books
             .Include(b => b.Publisher)
             .Include(d => d.Discount);
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
            query = query.Where(s => s.Id == id).Include(b => b.Publisher).Include(d => d.Discount);
            return View(query);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Book model)
        {
            var selectedGenres = _bookService.GetGenreByBookId(model.Id);
            model.PublisherList = _publisherService.List().Select(a => new SelectListItem { Text = a.PublisherName, Value = a.Id.ToString() });
            model.DiscountList = _discountService.List().Select(a => new SelectListItem { Text = a.DiscountPercentage.ToString(), Value = a.Id.ToString() });
            MultiSelectList selectListItemsGenre = new MultiSelectList(_genreService.List(), "Id", "GenreName", selectedGenres);
            model.MultiGenreList = selectListItemsGenre;
            var selectedAuthor = _bookService.GetAuthorByBookId(model.Id);
            MultiSelectList selectListItemsAuthor = new MultiSelectList(_auhorService.List(), "Id", "AuthorName", selectedAuthor);
            model.MultiAuthorList = selectListItemsAuthor;
           
            if (model.ImageFile != null)
            {
                var fileResult = this._fileService.SaveImage(model.ImageFile);
                if (fileResult.Item1 == 0)
                {
                    TempData["msg"] = "File could not saved";
                    return View(model);
                }
                var imageName = fileResult.Item2;
                model.Image = imageName;
            }
            var result = await _bookService.Update(model);
            if (result)
            {
                TempData["msg"] = "Added Successfully";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["msg"] = "Error on server side";
                return View(model);
            }
        }
        public async Task<IActionResult> Index(string searchString, string searchButton, string filterButton, string sortOrder, int? page)
        {   
            var data = await _bookService.List(searchString, searchButton, filterButton, sortOrder, page);

            ViewBag.TotalPages = data.TotalPages;
            ViewBag.CurrentPage = data.CurrentPage;
            ViewBag.CurrentSort = sortOrder;
            ViewBag.BookNameSortParm = String.IsNullOrEmpty(sortOrder) ? "BookName_desc" : "";
            ViewBag.PriceSortParm = sortOrder == "Price" ? "Price_desc" : "Price";
            ViewBag.ReleaseYearSortParm = sortOrder == "ReleaseYear" ? "ReleaseYear_desc" : "ReleaseYear";
            return View(data);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var result = await _bookService.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
