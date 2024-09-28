using AutoMapper;
using Library.Application.Contracts;
using Library.Application.Contracts.BookContracts;
using Library.Application.Exceptions;
using Library.Persistence.UnitOfWork;
using Microsoft.Extensions.Configuration;

namespace Library.Application.Services.BookUseCases;

public class GetBookByIdUseCase(
    IUnitOfWork unitOfWork,
    IMapper mapper,
    IConfiguration configuration)
{
    private readonly string _baseUrl = configuration["ImageBaseUrl"]!;
    
    public async Task<BookResponse> ExecuteAsync(int id)
    {
        var book = await unitOfWork.BooksRepository.GetById(id);
        
        if (book is null)
        {
            throw new ItemNotFoundException($"Book with id:{id} not found");
        }
        
        var bookResponse = mapper.Map<BookResponse>(book);
        var imagePath = bookResponse.ImageUrl;
        bookResponse.ImageUrl = _baseUrl + imagePath;
        
        return bookResponse;
    }
}