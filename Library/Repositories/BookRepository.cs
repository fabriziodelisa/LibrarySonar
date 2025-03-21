using Library.Data;
using Library.Models.Entities;

namespace Library.Repositories
{
    public class BookRepository : Repository, IBookRepository
    {
        public BookRepository(LibraryContext libraryContext) : base(libraryContext)
        {
            
        }
        public IEnumerable<Book> GetAll()
        {
            return _libraryContext.Books.OrderBy(x => x.Author).ToList();
        }

        public Book? GetById(int id)
        {
            return _libraryContext.Books.Find(id);
        }

        public IEnumerable<Book> SearchByName(string searchTerm)
        {
            return _libraryContext.Books
                .Where(b => b.Title.ToLower().Contains(searchTerm.ToLower()) ||
                            b.Author.ToLower().Contains(searchTerm.ToLower()))
                .ToList();
        }

        public Book Add(Book book)
        {
                var bookAdded = _libraryContext.Books.Add(book);
                SaveChange();
                return bookAdded.Entity;
        }

        public bool Available(bool available, int id)
        {
            var book = _libraryContext.Books.Find(id);
            if (book == null) 
                throw new KeyNotFoundException("Book was not found.");
            book.Available = available;
            SaveChange();
            return book.Available;
        }

        public Book? Delete(int id)
        {
            var bookToDelete = _libraryContext.Books.Find(id);
            if(bookToDelete != null)
            {
                var deletedBook = _libraryContext.Books.Remove(bookToDelete);
                SaveChange();
                return deletedBook.Entity;
            }
            return bookToDelete;
        }
    }
}
