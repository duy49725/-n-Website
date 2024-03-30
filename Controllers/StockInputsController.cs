using DoAnWebNangCao.Data;
using DoAnWebNangCao.Models;
using DoAnWebNangCao.Repositories.Abstraction;
using DoAnWebNangCao.Repositories.Implementation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace DoAnWebNangCao.Controllers
{
    public class StockInputsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IStockInputService _stockInputService;
        public StockInputsController(ApplicationDbContext context, IStockInputService stockInputService)
        {
            _context = context;
            _stockInputService = stockInputService;
        }
        public IActionResult Create()
        {
            ViewData["BookId"] = new SelectList(_context.Books, "Id", "BookName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Date,BookId,Quantity,UnitPrice")] StockInput stockInput)
        {
            ModelState.Remove("Book");
            if (ModelState.IsValid)
            {
                var result = await _stockInputService.Add(stockInput);
                return RedirectToAction(nameof(Index));
            }
            ViewData["BookId"] = new SelectList(_context.Books, "Id", "BookName", stockInput.BookId);
            return View(stockInput);
        }

        public async Task<IActionResult> Index(DateTime searchString, string searchButton, string filterButton, int? page)
        {
            var data = await _stockInputService.List(searchString, searchButton, filterButton, page);

            ViewBag.TotalPages = data.TotalPages;
            ViewBag.CurrentPage = data.CurrentPage;

            return View(data);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.StockInputs == null)
            {
                return NotFound();
            }

            var stockInput = await _context.StockInputs.FindAsync(id);
            if (stockInput == null)
            {
                return NotFound();
            }
            ViewData["BookId"] = new SelectList(_context.Books, "Id", "BookName", stockInput.BookId);
            return View(stockInput);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Date,BookId,Quantity,UnitPrice")] StockInput stockInput)
        {
            if (id != stockInput.Id)
            {
                return NotFound();
            }
            ModelState.Remove("Book");
            if (ModelState.IsValid)
            {
                try
                {
                    var result = await _stockInputService.Update(stockInput);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StockInputExists(stockInput.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["BookId"] = new SelectList(_context.Books, "Id", "BookName", stockInput.BookId);
            return View(stockInput);
        }
        private bool StockInputExists(int id)
        {
            return (_context.StockInputs?.Any(e => e.Id == id)).GetValueOrDefault();
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.StockInputs == null)
            {
                return NotFound();
            }

            var stockInput = await _context.StockInputs
                .Include(s => s.Book)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (stockInput == null)
            {
                return NotFound();
            }

            return View(stockInput);
        }
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _stockInputService.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
