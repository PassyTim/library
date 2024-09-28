using AutoMapper;
using Library.Application.Contracts;
using Library.Application.Contracts.BookContracts;
using Library.Persistence.UnitOfWork;
using Microsoft.Extensions.Configuration;

namespace Library.Application.Services.BookUseCases;

public class GetAllBooksUseCase(
    IUnitOfWork unitOfWork,
    IMapper mapper,
    IConfiguration configuration)
{
    private readonly string _baseUrl = configuration["ImageBaseUrl"]!;
    
    public async Task<List<BookResponse>> ExecuteAsync(int pageSize = 0, int pageNumber = 0)
    {
        if (pageSize > 100) pageSize = 100;
        
        var books = await unitOfWork.BooksRepository.GetAllAsync(null, pageSize, pageNumber);
        var booksResponse = mapper.Map<List<BookResponse>>(books);
        foreach (var item in booksResponse)
        {
            var imagePath = item.ImageUrl;
            item.ImageUrl = _baseUrl + imagePath;
        }

        return booksResponse;
    }
}