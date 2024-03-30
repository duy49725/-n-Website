using DoAnWebNangCao.Models;

namespace DoAnWebNangCao.Repositories.Abstraction
{
    public interface IHomeService
    {
        Task<IEnumerable<Book>> GetAllBooks(string sTerm = "");
        Task<IEnumerable<Genre>> Genres();
        Task<Book> GetById(int id);
        Task<IEnumerable<Book>> GetDiscountedBooks();
    }
}
