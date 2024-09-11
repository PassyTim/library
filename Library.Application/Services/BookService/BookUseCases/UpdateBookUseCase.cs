using AutoMapper;
using FluentValidation;
using Library.Application.Contracts;
using Library.Domain.Models;
using Library.Persistence;

namespace Library.Application.Services.BookService.BookUseCases;

public class UpdateBookUseCase(
    IValidator<BookRequest> validator,
    IMapper mapper,
    IUnitOfWork unitOfWork)
{
    public async Task ExecuteAsync(int id, BookRequest bookUpdate)
    {
        var context = new ValidationContext<BookRequest>(bookUpdate);
        context.RootContextData["Id"] = id;
        context.RootContextData["IsUpdate"] = true;
        var validationResult = await validator.ValidateAsync(context);
        
        if (!validationResult.IsValid)
        {
            var errorMessages = validationResult.Errors.Select(x => x.ErrorMessage);
            throw new ValidationException(string.Join(". ", errorMessages));
        }
        
        FileUploadHandler fileUploadHandler = new FileUploadHandler();
        var path = fileUploadHandler.Upload(bookUpdate.Image);
        
        var bookToUpdate = mapper.Map<Book>(bookUpdate);
        var isbn = bookToUpdate.Isbn;

        bookToUpdate.ImagePath = path;
        bookToUpdate.Isbn = IsbnNormalizer.NormalizeIsbn(isbn);
        bookToUpdate.UserId = null;
        await unitOfWork.BooksRepository.UpdateAsync(bookToUpdate);
        await unitOfWork.SaveChangesAsync();
    }
}