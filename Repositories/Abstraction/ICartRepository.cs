using DoAnWebNangCao.Models;

namespace DoAnWebNangCao.Repositories.Abstraction
{
    public interface ICartRepository
    {
        Task<int> AddItem(int BookId, int qty);
        Task<int> DeleteItem(int BookId);
        Task<Cart> GetUserCart();
        Task<int> GetCartItemCount(string userId = "");
        Task<Cart> GetCart(string userId);
        Task<bool> DoCheckOut();
        Task<bool> ApplyCouponToCart(Coupon coupon);
    }
}
