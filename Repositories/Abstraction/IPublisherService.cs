using DoAnWebNangCao.Data.Paging;
using DoAnWebNangCao.Models;

namespace DoAnWebNangCao.Repositories.Abstraction
{
    public interface IPublisherService
    {
        Task<bool> Add(Publisher model);
        Task<bool> Update(Publisher model);
        Task<Publisher> GetById(int id);
        Task<bool> Delete(int id);
        Task<PublisherPaging> List(string searchString, string searchButton, string filterButton, string sortOrder, int? page);
        IQueryable<Publisher> List();
    }
}
