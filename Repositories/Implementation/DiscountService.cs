using DoAnWebNangCao.Data.Paging;
using DoAnWebNangCao.Data;
using DoAnWebNangCao.Models;
using Microsoft.EntityFrameworkCore;
using DoAnWebNangCao.Repositories.Abstraction;

namespace DoAnWebNangCao.Repositories.Implementation
{
    public class DiscountService : IDiscountService
    {
        private readonly ApplicationDbContext _context;
        public DiscountService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Add(Discount model)
        {
            try
            {
                _context.Discounts.Add(model);
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
                var data = await _context.Discounts.FindAsync(id);
                if (data == null)
                    return false;
                _context.Discounts.Remove(data);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<Discount> GetById(int id)
        {
            return await _context.Discounts.FindAsync(id);
        }
        public IQueryable<Discount> List()
        {
            var data = _context.Discounts.AsQueryable();
            return data;
        }

        public async Task<DiscountPaging> List(string searchString, string searchButton, string filterButton, string sortOrder, int? page)
        {
            var query = from g in _context.Discounts
                        select g;
            if (!string.IsNullOrEmpty(searchButton) && !String.IsNullOrEmpty(searchString))
            {
                query = query.Where(s => s.Name.Contains(searchString));
            }

            query = sortOrder switch
            {
                "DiscountName_desc" => query.OrderByDescending(s => s.Name),
                _ => query.OrderBy(s => s.Name)
            };
            int pageSize = 8;
            int pageNumber = page ?? 1;
            var discountPaging = new DiscountPaging
            {
                Discounts = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync()
            };
            discountPaging.TotalPages = (int)Math.Ceiling(await query.CountAsync() / (double)pageSize);
            discountPaging.CurrentPage = pageNumber;
            return discountPaging;
        }

        public async Task<bool> Update(Discount model)
        {
            try
            {
                _context.Discounts.Update(model);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
