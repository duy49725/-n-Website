using DoAnWebNangCao.Data.Paging;
using DoAnWebNangCao.Models;

namespace DoAnWebNangCao.Repositories.Abstraction
{
    public interface IStockInputService
    {
        Task<bool> Add(StockInput stockInput);
        Task<bool> Update(StockInput stockInput);
        Task<StockInput> GetById(int id);
        Task<bool> Delete(int id);
        Task<StockInputPaging> List(DateTime searchString, string searchButton, string filterButton, int? page);
        IQueryable<StockInput> List();
    }
}
