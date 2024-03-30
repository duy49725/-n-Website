using DoAnWebNangCao.Data.Paging;
using DoAnWebNangCao.Data;
using DoAnWebNangCao.Models;
using Microsoft.EntityFrameworkCore;
using DoAnWebNangCao.Repositories.Abstraction;

namespace DoAnWebNangCao.Repositories.Implementation
{
    public class OrdersStatusService:IOrdersStatusService
    {
        private readonly ApplicationDbContext _context;
        public OrdersStatusService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Add(OrderStatus model)
        {
            try
            {
                _context.OrderStatus.Add(model);
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
                var data = await _context.OrderStatus.FindAsync(id);
                if (data == null)
                    return false;
                _context.OrderStatus.Remove(data);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<OrderStatus> GetById(int id)
        {
            return await _context.OrderStatus.FindAsync(id);
        }
        public IQueryable<OrderStatus> List()
        {
            var data = _context.OrderStatus.AsQueryable();
            return data;
        }

        public async Task<OrdersStatusPaging> List(string searchString, string searchButton, string filterButton, string sortOrder, int? page)
        {
            var query = from g in _context.OrderStatus
                        select g;
            if (!string.IsNullOrEmpty(searchButton) && !String.IsNullOrEmpty(searchString))
            {
                query = query.Where(s => s.StatusName.Contains(searchString));
            }

            query = sortOrder switch
            {
                "StatusName_desc" => query.OrderByDescending(s => s.StatusName),
                _ => query.OrderBy(s => s.StatusName)
            };
            int pageSize = 8;
            int pageNumber = page ?? 1;
            var ordersStatusPaging = new OrdersStatusPaging
            { 
                OrderStatuses = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync()
            };
            ordersStatusPaging.TotalPages = (int)Math.Ceiling(await query.CountAsync() / (double)pageSize);
            ordersStatusPaging.CurrentPage = pageNumber;
            return ordersStatusPaging;
        }

        public async Task<bool> Update(OrderStatus model)
        {
            try
            {
                _context.OrderStatus.Update(model);
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
