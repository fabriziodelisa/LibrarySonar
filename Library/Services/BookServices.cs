using AutoMapper;
using Library.Models.DTOs;
using Library.Models.Entities;
using Library.Models.Responses;
using Library.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Library.Services
{
    public class BookServices : IBookServices
    {
        internal readonly IBookRepository _bookRepository;
        internal readonly IMapper _mapper;
        public BookServices( IBookRepository booksRepository, IMapper mapper)
        {
            _bookRepository = booksRepository;
            _mapper = mapper;
        }

        public ServiceResponse<IEnumerable<BookDTO>> GetAll()
        {
            var response = new ServiceResponse<IEnumerable<BookDTO>>();

            try
            {
                var allBooks = _bookRepository.GetAll();
                response.Data = _mapper.Map<IEnumerable<BookDTO>>(allBooks);
                response.Message = "Books retrieved successfully";
                
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "Error retrieving books";
                response.AddError(ex.Message);
            }

            return response;
        }

        public ServiceResponse<BookDTO> GetById(int id)
        {
            var response = new ServiceResponse<BookDTO>();

            try
            {
                var book = _bookRepository.GetById(id);
                if (book == null)
                {
                    response.Success = false;
                    response.Message = $"Book with ID {id} was not found";
                    return response;
                }
                else
                {
                    response.Message = "Book retrieved successfully";
                    response.Data = _mapper.Map<BookDTO>(book);
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "Error retrieving book";
                response.AddError(ex.Message);
            }
            return response;
        }

        public ServiceResponse<IEnumerable<BookDTO>> SearchByName(string name)
        {
            var response = new ServiceResponse<IEnumerable<BookDTO>>();
            try
            {
                if (string.IsNullOrWhiteSpace(name))
                {
                    response.Success = false;
                    response.Message = "Search term cannot be empty";
                    response.Data = [];
                    return response;
                }

                var books = _bookRepository.SearchByName(name);

                response.Data = _mapper.Map<IEnumerable<BookDTO>>(books);

                if (!books.Any())
                {
                    response.Message = $"No books found matching '{name}'";
                }
                else
                {
                    response.Message = $"Found {books.Count()} books matching '{name}'";
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "Error searching for books";
                response.AddError(ex.Message);
                response.Data = [];
            }

            return response;
        }

        public ServiceResponse<BookDTO> Add(CreateBookDTO book)
        {
            var response = new ServiceResponse<BookDTO>();

            if (book == null)
            {
                response.AddError("Book data cannot be null");
                return response;
            }

            if (string.IsNullOrEmpty(book.Title))
                response.AddError("The title is required");

            if (string.IsNullOrEmpty(book.Author))
                response.AddError("The author is required");

            if (!response.Success)
                return response;

            try
            {
                var newBook = _mapper.Map<Book>(book);
                newBook.RegistrationDate = DateTime.UtcNow;
                newBook.Available = true;

                var addedBook = _bookRepository.Add(newBook);

                response.Data = _mapper.Map<BookDTO>(addedBook);
                response.Message = "Book added successfully";
                return response;
            }
            catch (DbUpdateException ex)
            {
                // Error de base de datos
                response.Success = false;
                response.Message = "Error saving to database";
                response.AddError(ex.Message);

                return response;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "An unexpected error occurred";
                response.AddError(ex.Message);

                return response;
            }
        }

        public ServiceResponse<bool> Available(bool available, int id)
        {
            var response = new ServiceResponse<bool>();
            try
            {
                var book = _bookRepository.GetById(id);
                if (book == null)
                {
                    response.Success = false;
                    response.Message = $"Book with ID {id} was not found";
                    return response;
                }

                bool isAvailable = _bookRepository.Available(available, id);
                response.Data = isAvailable;
                response.Message = $"The book with id {id} is now {(isAvailable ? "Available" : "Unavailable")}";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "Error updating book availability";
                response.AddError(ex.Message);
            }

            return response;
        }

        public ServiceResponse<BookDTO> Delete(int id)
        {
            var response = new ServiceResponse<BookDTO>();

            try
            {
                var deletedBook = _bookRepository.Delete(id);
            
                if (deletedBook is null)
                {
                    response.Success = false;
                    response.Message = $"There is no book with the ID {id}";
                    return response;
                }
                response.Success = true;
                response.Message = $"{deletedBook.Title} was deleted successfully";
                response.Data = _mapper.Map<BookDTO>(deletedBook);
                return response;

            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "internal error deleting book";
                response.AddError(ex.Message);
                return response;
            }
        }
    }
}
