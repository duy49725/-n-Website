using DoAnWebNangCao.Data;
using DoAnWebNangCao.Migrations;
using DoAnWebNangCao.Models;
using DoAnWebNangCao.Repositories.Abstraction;
using DoAnWebNangCao.Repositories.Implementation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DoAnWebNangCao.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartRepository _cartRepository;
        private readonly ICouponRepository _couponRepository;
        private readonly ApplicationDbContext _context;
        private readonly ILogger<CartController> _logger;
        public CartController(ILogger<CartController> logger, ICartRepository cartRepository, ApplicationDbContext context, ICouponRepository couponRepository)
        {
            _cartRepository = cartRepository;
            _context = context;
            _couponRepository = couponRepository;
            _logger = logger;

        }

        public async Task<IActionResult> AddItem(int bookId, int qty = 1)
        {
            var cartCount = await _cartRepository.AddItem(bookId, qty);
            var cart = await _cartRepository.GetUserCart();

            var subtotal = cart.CartDetails.Sum(item => item.Quantity * item.Book.Price);
            double discountAmount = 0;
            if (cart.CouponId.HasValue)
            {
                var coupon = await _context.Coupons.FindAsync(cart.CouponId.Value);
                if (coupon != null && coupon.IsActive)
                {
                    discountAmount = subtotal * (coupon.DiscountPercentage / 100);
                }
            }
            var totalAmount = subtotal - discountAmount;

            ViewBag.DiscountAmount = discountAmount;
            ViewBag.TotalAmount = totalAmount;
            ViewBag.Subtotal = subtotal;

            return PartialView("_CartPartialView", cart);
        }

        public async Task<IActionResult> DeleteItem(int bookId)
        {
            var cartCount = await _cartRepository.DeleteItem(bookId);
            var cart = await _cartRepository.GetUserCart();

            var subtotal = cart.CartDetails.Sum(item => item.Quantity * item.Book.Price);
            double discountAmount = 0;
            if (cart.CouponId.HasValue)
            {
                var coupon = await _context.Coupons.FindAsync(cart.CouponId.Value);
                if (coupon != null && coupon.IsActive)
                {
                    discountAmount = subtotal * (coupon.DiscountPercentage / 100);
                }
            }
            var totalAmount = subtotal - discountAmount;

            ViewBag.DiscountAmount = discountAmount;
            ViewBag.TotalAmount = totalAmount;
            ViewBag.Subtotal = subtotal;

            return PartialView("_CartPartialView", cart);
        }


        public async Task<IActionResult> GetUserCart()
        {
            var cart = await _cartRepository.GetUserCart();
            var subtotal = cart.CartDetails.Sum(item => item.Quantity * item.Book.Price);
            double discountAmount = 0;
            if (cart.CouponId.HasValue)
            {
                var coupon = await _context.Coupons.FindAsync(cart.CouponId.Value);
                if (coupon != null && coupon.IsActive)
                {
                    discountAmount = subtotal * (coupon.DiscountPercentage / 100);
                }
            }
            var totalAmount = subtotal - discountAmount;

            ViewBag.Subtotal = subtotal;
            ViewBag.DiscountAmount = discountAmount;
            ViewBag.TotalAmount = totalAmount;

            return View(cart);
        }

        public async Task<IActionResult> GetTotalItemInCart()
        {
            int cartItem = await _cartRepository.GetCartItemCount();
            return Ok(cartItem);
        }
        public async Task<IActionResult> CheckOut()
        {
            var isCheckOut = await _cartRepository.DoCheckOut();
            if (!isCheckOut)
                throw new Exception("SomeThing happen in server side");
            return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        public async Task<IActionResult> ApplyCoupon(string couponCode)
        {
            var coupon = await _couponRepository.GetValidCoupon(couponCode);
            if (coupon != null && coupon.IsActive)
            {
                // Áp dụng coupon cho giỏ hàng
                var isSuccess = await _cartRepository.ApplyCouponToCart(coupon);
                if (isSuccess)
                {
                    var cart = await _cartRepository.GetUserCart();
                    var subtotal = cart.CartDetails.Sum(item => item.Quantity * item.Book.Price);
                    double discountAmount = subtotal * (coupon.DiscountPercentage / 100);
                    var totalAmount = subtotal - discountAmount;
                    return Json(new { success = true, discountAmount = discountAmount, totalAmount = totalAmount });
                }
                else
                {
                    return Json(new { success = false, message = "Failed to apply coupon" });
                }
            }
            else
            {
                return Json(new { success = false, message = "Invalid coupon code or expired" });
            }
        }

        public async Task<IActionResult> GetTotalAmount()
        {
            var cart = await _cartRepository.GetUserCart();
            double totalAmount = CalculateTotalAmount(cart);
            ViewBag.TotalAmount = totalAmount;
            return PartialView("_CartTotalPartial", totalAmount);
        }

        private double CalculateTotalAmount(Cart cart)
        {
            double totalAmount = 0.05;
            if (cart != null && cart.CartDetails != null)
            {
                foreach (var item in cart.CartDetails)
                {
                    totalAmount += item.Quantity * item.Book.Price;
                }
            }
            return totalAmount;
        }

    }
}
