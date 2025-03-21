using AutoMapper;
using Library.Models.DTOs;
using Library.Models.Entities;

namespace Library.Map
{
    public class LibraryProfile : Profile
    {
        public LibraryProfile() 
        {        
            CreateMap<Book,BookDTO>();
            CreateMap<CreateBookDTO,Book>();
        }
    }
}
