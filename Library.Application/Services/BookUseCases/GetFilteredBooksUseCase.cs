using System.Linq.Expressions;
using AutoMapper;
using Library.Application.Contracts;
using Library.Domain.Models;
using Library.Persistence;
using Microsoft.Extensions.Configuration;

namespace Library.Application.Services.BookUseCases;

public class GetFilteredBooksUseCase(
    IUnitOfWork unitOfWork,
    IMapper mapper,
    IConfiguration configuration)
{
    private readonly string _baseUrl = configuration["ImageBaseUrl"]!;
    
    public async Task<List<BookResponse>> ExecuteAsync(int pageSize = 0, int pageNumber = 1, int? authorId = null, string? name = null)
    {
        if (pageSize > 100) pageSize = 100;
        
        Expression<Func<Book, bool>>? filter = null;

        if (authorId.HasValue || !string.IsNullOrEmpty(name))
        {
            filter = book =>
                (!authorId.HasValue || book.AuthorId == authorId.Value) &&
                (string.IsNullOrEmpty(name) || book.Name.Contains(name));
        }

        var books = await unitOfWork.BooksRepository.GetAllAsync(filter, pageSize, pageNumber);
        var booksResponse = mapper.Map<List<BookResponse>>(books);

        foreach (var item in booksResponse)
        {
            var imagePath = item.ImageUrl;
            item.ImageUrl = _baseUrl + imagePath;
        }

        return booksResponse;
    }
}