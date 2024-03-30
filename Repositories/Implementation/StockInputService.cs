using DoAnWebNangCao.Data.Paging;
using DoAnWebNangCao.Data;
using DoAnWebNangCao.Models;
using Microsoft.EntityFrameworkCore;
using DoAnWebNangCao.Repositories.Abstraction;

namespace DoAnWebNangCao.Repositories.Implementation
{
    public class StockInputService : IStockInputService
    {
        private readonly ApplicationDbContext _context;
        public StockInputService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Add(StockInput stockInput)
        {
            try
            {
                _context.Add(stockInput);
                await _context.SaveChangesAsync();
                var book = await _context.Books.FindAsync(stockInput.BookId);
                book.StockQuantity += stockInput.Quantity;
                _context.Update(book);
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
                var data = await _context.StockInputs.FindAsync(id);
                if (data == null)
                    return false;
                _context.StockInputs.Remove(data);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<StockInput> GetById(int id)
        {
            return await _context.StockInputs.FindAsync(id);
        }
        public IQueryable<StockInput> List()
        {
            var data = _context.StockInputs.AsQueryable();
            return data;
        }

        public async Task<StockInputPaging> List(DateTime searchString, string searchButton, string filterButton, int? page)
        {
            var query = from g in _context.StockInputs
                        select g;
            if (!string.IsNullOrEmpty(searchButton))
            {
                query = query.Where(s => s.Date.Date == searchString.Date);
            }
            int pageSize = 8;
            int pageNumber = page ?? 1;
            var stockinputPaging = new StockInputPaging
            {
                StockInputs = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync()
            };
            stockinputPaging.TotalPages = (int)Math.Ceiling(await query.CountAsync() / (double)pageSize);
            stockinputPaging.CurrentPage = pageNumber;
            return stockinputPaging;
        }

        public async Task<bool> Update(StockInput stockInput)
        {
            try
            {
                _context.Update(stockInput);
                await _context.SaveChangesAsync();
                var book = await _context.Books.FindAsync(stockInput.BookId);
                book.StockQuantity += stockInput.Quantity;
                _context.Update(book);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
