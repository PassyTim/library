using AutoMapper;
using FluentValidation;
using Library.Application.Contracts;
using Library.Application.Exceptions;
using Library.Persistence;
using Microsoft.Extensions.Configuration;

namespace Library.Application.Services.BookUseCases;

public class GetBookByIsbnUseCase(
    IUnitOfWork unitOfWork,
    IMapper mapper,
    IConfiguration configuration)
{
    private readonly string _baseUrl = configuration["ImageBaseUrl"]!;
    public async Task<BookResponse> ExecuteAsync(string isbn)
    {
        var book = await unitOfWork.BooksRepository.GetByIsbn(isbn);

        if (!IsbnValidator.Validate(isbn))
        {
            throw new ValidationException("Invalid ISBN");
        }
        if (book is null)
        {
            throw new ItemNotFoundException($"Book with isbn:{isbn} not found");
        }
        
        var bookResponse = mapper.Map<BookResponse>(book);
        var imagePath = bookResponse.ImageUrl;
        bookResponse.ImageUrl = _baseUrl + imagePath;
        
        return bookResponse;
    }
}