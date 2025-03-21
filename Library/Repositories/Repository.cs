using Library.Data;

namespace Library.Repositories
{
    public class Repository : IRepository
    {
        internal readonly LibraryContext _libraryContext;
        public Repository(LibraryContext libraryContext)
        { 
            _libraryContext = libraryContext;
        }
        public bool SaveChange()
        {
            return _libraryContext.SaveChanges() >= 0;
        }
    }
}
