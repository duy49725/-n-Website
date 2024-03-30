using DoAnWebNangCao.Data;
using DoAnWebNangCao.Data.Paging;
using DoAnWebNangCao.Models;
using DoAnWebNangCao.Repositories.Abstraction;
using Microsoft.EntityFrameworkCore;

namespace DoAnWebNangCao.Repositories.Implementation
{
    public class CouponRepository : ICouponRepository
    {
        private readonly ApplicationDbContext _context;

        public CouponRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Coupon> GetValidCoupon(string couponCode)
        {
            var today = DateTime.Today;
            var coupon = await _context.Coupons
                .Where(c => c.Code == couponCode && c.IsActive && c.StartDate <= today && c.EndDate >= today)
                .FirstOrDefaultAsync();
            Console.WriteLine("Coupon duy nene"+coupon);
            return coupon;
        }

        public async Task<bool> Add(Coupon model)
        {
            try
            {
                _context.Coupons.Add(model);
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
                var data = await _context.Coupons.FindAsync(id);
                if (data == null)
                {
                    return false;
                }
                _context.Coupons.Remove(data);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<Coupon> GetById(int id)
        {
            return await _context.Coupons.FindAsync(id);
        }

        public IQueryable<Coupon> List()
        {
            var data = _context.Coupons.AsQueryable();
            return data;
        }

        public async Task<CouponPaging> List(string searchString, string searchButton, string filterButton, string sortOrder, int? page)
        {
            var query = from g in _context.Coupons
                        select g;
            if(!string.IsNullOrEmpty(searchButton) && !String.IsNullOrEmpty(searchString))
            {
                query = query.Where(s => s.Code.Contains(searchString));
            }

            query = sortOrder switch
            {
                "Code_des" => query.OrderByDescending(s => s.Code),
                _ => query.OrderBy(s => s.Code)
            };
            int pageSize = 8;
            int pageNumber = page ?? 1;
            var couponPaging = new CouponPaging 
            {
                Coupons = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync()
            };
            couponPaging.TotalPages = (int)Math.Ceiling(await query.CountAsync() / (double)pageSize);
            couponPaging.CurrentPage = pageNumber;
            return couponPaging;
        }

        public async Task<bool> Update(Coupon model)
        {
            try
            {
                _context.Coupons.Update(model);
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
