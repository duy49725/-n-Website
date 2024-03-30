using DoAnWebNangCao.Data.Paging;
using DoAnWebNangCao.Models;

namespace DoAnWebNangCao.Repositories.Abstraction
{
    public interface ICouponRepository
    {
        Task<Coupon> GetValidCoupon(string couponCode);
        Task<bool> Add(Coupon model);
        Task<bool> Update(Coupon model);
        Task<bool> Delete(int id);
        Task<Coupon> GetById(int id);
        Task<CouponPaging> List(string searchString, string searchButton, string filterButton, string sortOrder, int? page);
        IQueryable<Coupon> List();
    }
}
