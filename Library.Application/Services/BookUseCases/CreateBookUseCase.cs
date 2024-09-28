using AutoMapper;
using FluentValidation;
using Library.Application.Contracts;
using Library.Application.Contracts.BookContracts;
using Library.Domain.Models;
using Library.Persistence.UnitOfWork;

namespace Library.Application.Services.BookUseCases;

public class CreateBookUseCase(
    IMapper mapper,
    IUnitOfWork unitOfWork)
{
    public async Task ExecuteAsync(BookRequest bookCreate)
    {
        if (bookCreate.Id != 0)
        {
            throw new ValidationException("Id must be 0 while creating book");
        }

        if (!await IsIsbnUniqueAsync(bookCreate))
        {
            throw new ValidationException("Isbn already exists");
        }
            
        FileUploadHandler fileUploadHandler = new FileUploadHandler();
        var path = fileUploadHandler.Upload(bookCreate.Image);
        
        var bookToCreate = mapper.Map<Book>(bookCreate);
        var isbn = bookToCreate.Isbn;

        bookToCreate.ImagePath = path;
        bookToCreate.Isbn = IsbnNormalizer.NormalizeIsbn(isbn);
        
        await unitOfWork.BooksRepository.CreateAsync(bookToCreate);
        await unitOfWork.SaveChangesAsync();
    }
    
    private async Task<bool> IsIsbnUniqueAsync(BookRequest bookRequest)
    {
        var book = await unitOfWork.BooksRepository.GetByIsbn(bookRequest.Isbn);
        
        return book is null;
    }
}