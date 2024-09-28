using Library.Application.Contracts;
using Library.Application.Contracts.BookContracts;
using Library.Application.Exceptions;
using Library.Domain.Models;
using Library.Persistence.UnitOfWork;
using Microsoft.AspNetCore.Identity;

namespace Library.Application.Services.BookUseCases;

public class ReturnBookUseCase(
    IUnitOfWork unitOfWork,
    UserManager<User> userManager)
{
    public async Task ExecuteAsync(ReturnBookRequest returnBookRequest)
    {
        var book = await unitOfWork.BooksRepository.GetById(returnBookRequest.BookId);
        var user = await userManager.FindByIdAsync(returnBookRequest.UserId);

        if (book is null || user is null)
        {
            throw new ItemNotFoundException("Book or user not found");
        }

        book.ReturnDate = DateTime.Today;
        book.TakeDate = DateTime.Today;
        book.UserId = null;
        
        await unitOfWork.BooksRepository.UpdateAsync(book);
        await unitOfWork.SaveChangesAsync();
    }
}