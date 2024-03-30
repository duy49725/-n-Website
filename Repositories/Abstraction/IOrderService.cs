using DoAnWebNangCao.Models;

namespace DoAnWebNangCao.Repositories.Abstraction
{
    public interface IOrderService
    {
        Task<IEnumerable<Order>> UserOrder();
        Task<IEnumerable<Order>> Order();
        Task<bool> Update(Order model);
		Task<Order> GetById(int id);

	}
}
