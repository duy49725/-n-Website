using DoAnWebNangCao.Data;
using DoAnWebNangCao.Models;
using DoAnWebNangCao.Repositories.Abstraction;
using DoAnWebNangCao.Repositories.Implementation;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace DoAnWebNangCao.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;
		private readonly ApplicationDbContext _context;
		private readonly UserManager<User> _userManager;
		public OrderController(IOrderService orderService, UserManager<User> userManager, ApplicationDbContext context)
        {
            _orderService = orderService;
            _userManager = userManager;
			_context = context;
        }

		public async Task<IActionResult> Edit(int id)
		{
			var data = await _orderService.GetById(id);
			ViewData["OrderStatusId"] = new SelectList(_context.OrderStatus, "Id", "StatusName", data.OrderStatusId);
			return View(data);
		}
		[HttpPost]
		public async Task<IActionResult> Edit(Order model)
		{
			var result = await _orderService.Update(model);
			if (result)
			{
				TempData["msg"] = "Added Successfully";
				return RedirectToAction(nameof(AdminOrder));
			}
			else
			{
				TempData["msg"] = "Error on server side";
				return View(model);
			}
		}
		public async Task<IActionResult> Order()
        {
            var orders = await _orderService.UserOrder();
            return View(orders);
        }

		public async Task<IActionResult> UserOrder(string userId, int orderId)
		{
			var orders = await _context.Order
				  .Include(x => x.OrderStatus)
				  .Include(x => x.OrderDetails)
				  .ThenInclude(x => x.Book)
				  .Where(a => a.UserId == userId && a.Id == orderId)
				  .ToListAsync();
			return View(orders);
		}

		public async Task<IActionResult> AdminOrder(Order order)
        {
            var orders = await _orderService.Order();
			return View(orders);
        }
    }
}
