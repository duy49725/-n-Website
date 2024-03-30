using DoAnWebNangCao.Data;
using DoAnWebNangCao.Models;
using DoAnWebNangCao.Repositories.Abstraction;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DoAnWebNangCao.Repositories.Implementation
{
    public class OrderService : IOrderService
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly UserManager<User> _userManager;
        public OrderService(ApplicationDbContext context, UserManager<User> userManager, IHttpContextAccessor contextAccessor)
        {
            _context = context;
            _contextAccessor = contextAccessor; 
            _userManager = userManager;
        }

        public async Task<IEnumerable<Order>> UserOrder()
        {
            var userId = GetUserId();
            if(string.IsNullOrEmpty(userId))
            {
                throw new Exception("User is loggin");
            }
            var orders = await _context.Order
                .Include(x => x.OrderStatus)
                .Include(x => x.OrderDetails)
                .ThenInclude(x => x.Book)
                .Where(a => a.UserId == userId)
                .ToListAsync();
            return orders;
        }

        public async Task<IEnumerable<Order>> Order()
        {
            var orders = await _context.Order
                .Include(x => x.OrderStatus)
                .Include(x => x.OrderDetails)
                .ThenInclude(x => x.Book)
                .ToListAsync();
			return orders;
        }

        public string GetUserId()
        {
            var principal = _contextAccessor.HttpContext.User;
            string userId = _userManager.GetUserId(principal);
            return userId;
        }
		public async Task<Order> GetById(int id)
		{
			return await _context.Order.FindAsync(id);
		}


		public async Task<bool> Update(Order model)
		{
			try
			{
				_context.Order.Update(model);
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
