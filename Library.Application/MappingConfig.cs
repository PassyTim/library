using AutoMapper;
using Library.Application.Contracts;
using Library.Application.Contracts.AuthContracts;
using Library.Application.Contracts.AuthorContracts;
using Library.Application.Contracts.BookContracts;
using Library.Domain.Models;

namespace Library.Application;

public class MappingConfig : Profile
{
    public MappingConfig()
    {
        CreateMap<Book, BookResponse>().ReverseMap()
            .ForMember(dest=> dest.ImagePath, 
                opt => opt.MapFrom(src => src.ImageUrl))
            .ForMember(b => b.Author,
                opts => opts.Ignore());

        CreateMap<BookResponse, Book>().ReverseMap()
            .ForMember(dest => dest.ImageUrl,
                opt => opt.MapFrom(src => src.ImagePath));
            
        CreateMap<BookRequest, Book>().ReverseMap();

        CreateMap<Author, AuthorRequest>().ReverseMap()
            .ForMember(dest => dest.Books,
                opts => opts.Ignore());
        CreateMap<AuthorResponse, Author>().ReverseMap();

        CreateMap<User, UserRegisterRequest>().ReverseMap();
        CreateMap<User, UserLoginRequest>().ReverseMap();
        CreateMap<ResponseUser, User>().ReverseMap();
        
        CreateMap<UserRegisterRequest, User>().ReverseMap()
            .ForMember(u => u.Role, 
                opts => opts.Ignore());
    }
}