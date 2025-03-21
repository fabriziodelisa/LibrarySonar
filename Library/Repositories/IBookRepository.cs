using Library.Models.Entities;

namespace Library.Repositories
{
    public interface IBookRepository
    {
        IEnumerable<Book> GetAll();
        Book Add(Book book);
        IEnumerable<Book> SearchByName(string name);
        bool Available(bool available, int id);
        Book? GetById(int id);
        Book? Delete(int id);
    }
}
