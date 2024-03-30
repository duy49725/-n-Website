using DoAnWebNangCao.Data;
using DoAnWebNangCao.Migrations;
using DoAnWebNangCao.Models;
using DoAnWebNangCao.Repositories.Abstraction;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DoAnWebNangCao.Repositories.Implementation
{
    public class CartRepository : ICartRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<CartRepository> _logger;
        private readonly UserManager<User> _userManager;
        private readonly IHttpContextAccessor _contextAccessor;
        public CartRepository(ApplicationDbContext context, ILogger<CartRepository> logger, UserManager<User> userManager, IHttpContextAccessor contextAccessor)
        {
            _context = context;
            _logger = logger;
            _userManager = userManager;
            _contextAccessor = contextAccessor;
        }
        private bool IsQuantityAvailable(int bookId, int quantity)
        {
            var book = _context.Books.Find(bookId);
            if (book == null)
            {
                return false; // Sách không tồn tại trong kho
            }

            return book.StockQuantity >= quantity;
        }
        public async Task<int> AddItem(int bookId, int qty)
        {
            string userId = GetUserId();
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                if (string.IsNullOrEmpty(userId))
                    throw new Exception("user is not loggin");
                if (!IsQuantityAvailable(bookId, qty))
                {
                    throw new Exception("Số lượng sách trong kho không đủ");
                }
                var cart = await GetCart(userId);
                if(cart is null)
                {
                    cart = new Cart 
                   { 
                        UserId = userId,                      
                   };
                    _context.Carts.Add(cart);
                }
                _context.SaveChanges();
                var cartItem = _context.CartDetails.FirstOrDefault(a => a.CartId == cart.Id && a.BookId == bookId);
                if (cartItem is not null)
                {
                    cartItem.Quantity += qty;
                }
                else
                {
                    var book = _context.Books.Find(bookId);
                    var unitPrice = book.Price;
                    Console.WriteLine("Discount Id la cai nay daydddds: " + book.DiscountId);
                    if (book.DiscountId.HasValue)
                    {
                        var discount = await _context.Discounts.FindAsync(book.DiscountId.Value);
                        Console.WriteLine("Discount Id: " + discount);
                        if (discount != null && discount.IsActive)
                        {
                            unitPrice = unitPrice * (1 - (double)(discount.DiscountPercentage / 100));
                        }
                    }
                    Console.WriteLine("Unit Price: " + unitPrice);
                    cartItem = new CartDetail
                    {
                        BookId = bookId,
                        CartId = cart.Id,
                        Quantity = qty,
                        UnitPrice = unitPrice
                    };
                    _context.CartDetails.Add(cartItem);
                }
                _context.SaveChanges();
                transaction.Commit();
            }
            catch (Exception ex)
            {

            }
            var cartItemCount = await GetCartItemCount(userId);
            Console.WriteLine("USer ID tr  nhat dy" + userId);
            return cartItemCount;
        }

        public async Task<int> DeleteItem(int bookId)
        {
            string userId = GetUserId();
            try
            {
                if (string.IsNullOrEmpty(userId)) 
                    throw new Exception("User is not Login");
                var cart = await GetCart(userId);
                if (cart == null)
                    throw new Exception("Invalid Cart");
                var cartItem = _context.CartDetails.FirstOrDefault(a => a.CartId == cart.Id && a.BookId == bookId);
                if (cartItem is null)
                    throw new Exception("Not Item in cart");
                else if (cartItem.Quantity == 1)
                    _context.CartDetails.Remove(cartItem);
                else
                    cartItem.Quantity = cartItem.Quantity - 1;
                _context.SaveChanges();
            }
            catch(Exception ex)
            {

            }
            var cartItemCount = await GetCartItemCount(userId);
            return cartItemCount;
        }

        public async Task<Cart> GetUserCart()
        {
            var userId = GetUserId();
            if (userId == null)
                throw new Exception("Invalid userId");
            var cart = await _context.Carts
                .Include(a => a.CartDetails)
                .ThenInclude(a => a.Book)
                .Where(a => a.UserId == userId)
                .FirstOrDefaultAsync();
            return cart;
        }

        public async Task<bool> DoCheckOut()
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                var userId = GetUserId();
				var userName = GetUserName();
				if (string.IsNullOrEmpty(userId))
                    throw new Exception("User is not Loggin");
                var cart = await GetCart(userId);
                if (cart is null)
                    throw new Exception("Invalid cart");
                var cartDetail = _context.CartDetails.Where(a => a.CartId == cart.Id).ToList();
                if (cartDetail.Count == 0)
                    throw new Exception("Cart is empty");
                await _context.SaveChangesAsync();
                double subTotal = cartDetail.Where(item => item.CartId == cart.Id).Sum(item => item.UnitPrice * item.Quantity);

                // Áp dụng giảm giá từ Coupon (nếu có)
                if (cart.CouponId.HasValue)
                {
                    var coupon = await _context.Coupons.FindAsync(cart.CouponId.Value);
                    if (coupon != null && coupon.IsActive)
                    {
                        subTotal = subTotal * (1 - (double)(coupon.DiscountPercentage / 100));
                    }
                }
                Console.WriteLine("Coupon ID la: "+ cart.CouponId);

                var order = new Order
                {
                    UserId = userId,
                    CreatedDate = DateTime.Now,
                    OrderStatusId = 4,
					UserName = userName,
                    TotalPrice = subTotal
			     };
                Console.WriteLine("User Name la:  " + userName);
                _context.Order.Add(order);
                _context.SaveChanges();
                foreach (var item in cartDetail)
                {
                    var orderDetail = new OrderDetail
                    {
                        BookId = item.BookId,
                        OrderId = order.Id,
                        Quantity = item.Quantity,
                        UnitPrice = item.UnitPrice
                    };
                    _context.OrderDetails.Add(orderDetail);
                }
                _context.SaveChanges();
                foreach (var item in order.OrderDetails)
                {
                    var book = await _context.Books.FindAsync(item.BookId);
                    if (book != null)
                    {
                        book.StockQuantity -= item.Quantity;
                        _context.Update(book);
                    }
                }

                await _context.SaveChangesAsync();
                cart.CouponId = null;
                _context.Update(cart);
                // removing the cartdetails
                _context.CartDetails.RemoveRange(cartDetail);
                _context.SaveChanges();
                transaction.Commit();
                return true;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error during checkout");
                transaction.Rollback();
                return false;
            }
        }
        public async Task<int> GetCartItemCount(string userId = "")
        {
            if (!string.IsNullOrEmpty(userId))
            {
                userId = GetUserId();
            }
            var data = await (from cart in _context.Carts
                              join cartDetail in _context.CartDetails
                              on cart.Id equals cartDetail.CartId
                              select new
                              {
                                  cartDetail.Id,
                              }).ToListAsync();
            return data.Count;
        }
        public async Task<Cart> GetCart(string userId)
        {
            var cart = await _context.Carts.FirstOrDefaultAsync(c => c.UserId == userId);
            return cart;
        }
        private string GetUserId()
        {
            var principal = _contextAccessor.HttpContext.User;
            string userId = _userManager.GetUserId(principal);
            return userId;
        }

		private string GetUserName()
		{
			var principal = _contextAccessor.HttpContext.User;
			string userName = _userManager.GetUserName(principal);
			return userName;
		}

		public async Task<bool> ApplyCouponToCart(Coupon coupon)
        {
            var userId = GetUserId();
            var cart = await GetCart(userId);
            if (cart == null)
            {
                return false;
            }

            // Áp dụng coupon vào giỏ hàng
            cart.CouponId = coupon.Id;
            _context.Update(cart);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
