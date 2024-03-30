using DoAnWebNangCao.Data.Paging;
using DoAnWebNangCao.Models;

namespace DoAnWebNangCao.Repositories.Abstraction
{
    public interface IDiscountService
    {
        Task<bool> Add(Discount model);
        Task<bool> Update(Discount model);
        Task<Discount> GetById(int id);
        Task<bool> Delete(int id);
        Task<DiscountPaging> List(string searchString, string searchButton, string filterButton, string sortOrder, int? page);
        IQueryable<Discount> List();
    }
}
