using AutoMapper;
using FluentValidation;
using Library.Application.Contracts;
using Library.Application.Contracts.BookContracts;
using Library.Domain.Models;
using Library.Persistence.UnitOfWork;

namespace Library.Application.Services.BookUseCases;

public class UpdateBookUseCase(
    IMapper mapper,
    IUnitOfWork unitOfWork)
{
    public async Task ExecuteAsync(int id, BookRequest bookUpdateRequest)
    {
        if (id != bookUpdateRequest.Id)
        {
            throw new ValidationException("The id's must match");
        }
        
        var book = await unitOfWork.BooksRepository.GetById(bookUpdateRequest.Id);
        if (book is null)
        {
            throw new ValidationException("Book to update not found");
        }

        if (!await IsIsbnUniqueAsync(bookUpdateRequest))
        {
            throw new ValidationException("Isbn already exists");
        }
        
        FileUploadHandler fileUploadHandler = new FileUploadHandler();
        var path = fileUploadHandler.Upload(bookUpdateRequest.Image);
        
        var bookToUpdate = mapper.Map<Book>(bookUpdateRequest);
        var isbn = bookToUpdate.Isbn;
        
        bookToUpdate.ImagePath = bookUpdateRequest.Image is null ? book.ImagePath : path;
        bookToUpdate.Isbn = IsbnNormalizer.NormalizeIsbn(isbn);
        bookToUpdate.UserId = null;
        await unitOfWork.BooksRepository.UpdateAsync(bookToUpdate);
        await unitOfWork.SaveChangesAsync();
    }

    private async Task<bool> IsIsbnUniqueAsync(BookRequest bookRequest)
    {
        var id = bookRequest.Id;
        var book = await unitOfWork.BooksRepository.GetByIsbn(bookRequest.Isbn);
        var existingBook = await unitOfWork.BooksRepository.GetById(id);
        
        return book is null || existingBook.Id == book.Id;
    }
}