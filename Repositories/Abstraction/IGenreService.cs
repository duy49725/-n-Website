using DoAnWebNangCao.Data.Paging;
using DoAnWebNangCao.Models;

namespace DoAnWebNangCao.Repositories.Abstraction
{
    public interface IGenreService
    {
        Task<bool> Add(Genre model);
        Task<bool> Update(Genre model);
        Task<Genre> GetById(int id);
        Task<bool> Delete(int id);
        Task<GenrePaging> List(string searchString, string searchButton, string filterButton, string sortOrder, int? page);
        IQueryable<Genre> List();
    }
}
