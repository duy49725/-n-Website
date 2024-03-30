using DoAnWebNangCao.Data.Paging;
using DoAnWebNangCao.Models;

namespace DoAnWebNangCao.Repositories.Abstraction
{
    public interface IOrdersStatusService
    {
        Task<bool> Add(OrderStatus model);
        Task<bool> Update(OrderStatus model);
        Task<OrderStatus> GetById(int id);
        Task<bool> Delete(int id);
        Task<OrdersStatusPaging> List(string searchString, string searchButton, string filterButton, string sortOrder, int? page);
        IQueryable<OrderStatus> List();
    }

}
