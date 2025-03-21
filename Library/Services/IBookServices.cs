using Library.Models.DTOs;
using Library.Models.Responses;

namespace Library.Services
{
    public interface IBookServices
    {
        ServiceResponse<IEnumerable<BookDTO>> GetAll();
        ServiceResponse<BookDTO> GetById(int id);
        ServiceResponse<IEnumerable<BookDTO>> SearchByName(string name);
        ServiceResponse<BookDTO> Add(CreateBookDTO book);
        ServiceResponse<bool> Available(bool available, int id);
        ServiceResponse<BookDTO> Delete(int id);
    }
}
