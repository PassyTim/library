using System.Threading.Tasks.Dataflow;
using AutoMapper;
using Library.Application.Contracts;
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

        CreateMap<AuthorRequest, Author>().ReverseMap();
        CreateMap<AuthorResponse, Author>().ReverseMap();

        CreateMap<User, UserRegisterRequest>().ReverseMap();
        CreateMap<User, UserLoginRequest>().ReverseMap();
        CreateMap<ResponseUser, User>().ReverseMap();
        
        CreateMap<UserRegisterRequest, User>().ReverseMap()
            .ForMember(u => u.Role, 
                opts => opts.Ignore());
    }
}