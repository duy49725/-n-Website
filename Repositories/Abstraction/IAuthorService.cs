using DoAnWebNangCao.Data.Paging;
using DoAnWebNangCao.Models;

namespace DoAnWebNangCao.Repositories.Abstraction
{
    public interface IAuthorService
    {
        Task<bool> Add(Author model);
        Task<bool> Update(Author model);
        Task<Author> GetById(int id);
        Task<bool> Delete(int id);
        Task<AuthorPaging> List(string searchString, string searchButton, string filterButton, string sortOrder, int? page);
        IQueryable<Author> List();
    }
}
