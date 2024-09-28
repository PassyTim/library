using Library.Application.Contracts;
using Library.Application.Contracts.BookContracts;
using Library.Application.Exceptions;
using Library.Domain.Models;
using Library.Persistence.UnitOfWork;
using Microsoft.AspNetCore.Identity;

namespace Library.Application.Services.BookUseCases;

public class TakeBookUseCase(
    IUnitOfWork unitOfWork,
    UserManager<User> userManager)
{
    public async Task ExecuteAsync(BookTakeRequest bookTakeRequest)
    {
        var book = await unitOfWork.BooksRepository.GetById(bookTakeRequest.BookId);
        var user = await userManager.FindByIdAsync(bookTakeRequest.UserId);

        if (book is null || user is null)
        {
            throw new ItemNotFoundException("Book or user not found");
        }

        book.ReturnDate = bookTakeRequest.ReturnDate;
        book.TakeDate = DateTime.Today;
        book.UserId = user.Id;

        await unitOfWork.BooksRepository.UpdateAsync(book);
        await unitOfWork.SaveChangesAsync();
    }
}