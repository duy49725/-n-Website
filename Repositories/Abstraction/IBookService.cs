using DoAnWebNangCao.Data.Paging;
using DoAnWebNangCao.Models;

namespace DoAnWebNangCao.Repositories.Abstraction
{
    public interface IBookService
    {
        Task<bool> Add(Book model);
        Task<bool> Update(Book model);
        Task<Book> GetById(int id);
        Task<bool> Delete(int id);
        Task<BookPaging> List(string searchString, string searchButton, string filterButton, string sortOrder, int? page);
        List<int> GetGenreByBookId(int bookId);
        List<int> GetAuthorByBookId(int bookId);
    }
}
