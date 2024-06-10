using AutoMapper;
using LibraryApp.Domain.Dto;
using LibraryApp.Domain.Models;

namespace LibraryApp.Core;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Author, AuthorDto>().ReverseMap();
        CreateMap<Author, GetAllAuthorDto>().ReverseMap();
        CreateMap<Book, BookDto>().ReverseMap();
        CreateMap<Book, GetAllBookDto>().ReverseMap();
        CreateMap<AuthorConnection, AuthorConnectionDto>().ReverseMap();
    }
}
