using System.Threading.Tasks.Dataflow;
using AutoMapper;
using Library.Application.Contracts;
using Library.Domain.Models;

namespace Library.Application;

public class MappingConfig : Profile
{
    public MappingConfig()
    {
        CreateMap<Book, BookResponse>(MemberList.None).ReverseMap()
            .ForMember(b => b.Author,
                opts => opts.Ignore())
            .ForMember(b => b.TakeDate,
                opts => opts.Ignore())
            .ForMember(b => b.ReturnDate,
                opts => opts.Ignore());
            
        CreateMap<BookRequest, Book>().ReverseMap();
    }
}