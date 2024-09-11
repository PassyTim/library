using AutoMapper;
using FluentValidation;
using Library.Application.Contracts;
using Library.Domain.Models;
using Library.Persistence;

namespace Library.Application.Services.BookService.BookUseCases;

public class CreateBookUseCase(
    IValidator<BookRequest> validator,
    IMapper mapper,
    IUnitOfWork unitOfWork)
{
    public async Task ExecuteAsync(BookRequest bookCreate)
    {
        var context = new ValidationContext<BookRequest>(bookCreate);
        context.RootContextData["IsCreate"] = true;
        var validationResult = await validator.ValidateAsync(context);
        
        if (!validationResult.IsValid)
        {
            var errorMessages = validationResult.Errors.Select(x => x.ErrorMessage);
            throw new ValidationException(string.Join(". ", errorMessages));
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
}