using Library.Application.Contracts;
using Library.Application.IServices;
using Library.Domain.Models;
using Library.Persistence;

namespace Library.Application.Services;

public class BorrowBookService(IUnitOfWork unitOfWork) : IBorrowBookService
{
    public async Task BorrowBookAsync(BorrowBookRequest borrowBookRequest)
    {
        var book = await unitOfWork.BooksRepository.GetById(borrowBookRequest.BookId);

        BorrowedBook borrowedBook = new BorrowedBook
        {
            BookId = borrowBookRequest.BookId,
            UserId = borrowBookRequest.UserId,
            ReturnDate = borrowBookRequest.ReturnDate,
            TakeDate = DateTime.Today
        };
        
        await unitOfWork.BorrowedBookRepository.CreateAsync(borrowedBook);
        
        book.AvailableCount--;
        await unitOfWork.BooksRepository.UpdateAsync(book);
        await unitOfWork.SaveChangesAsync();
    }

    public async Task<bool> TryReturnBookAsync(int id)
    {
        var borrowedBook = await unitOfWork.BorrowedBookRepository.GetByIdAsync(id);
        if (borrowedBook is null)
        {
            return false;
        }

        var book = await unitOfWork.BooksRepository.GetById(borrowedBook.BookId);
        book.AvailableCount++;
        await unitOfWork.BooksRepository.UpdateAsync(book);

        await unitOfWork.BorrowedBookRepository.RemoveAsync(borrowedBook.Id);
        await unitOfWork.SaveChangesAsync();

        return true;
    }

    public async Task<List<BorrowedBook>> GetAllByUserIdAsync(string userId)
    {
        var borrowedBooks = 
            await unitOfWork.BorrowedBookRepository.GetAllByUserId(userId);
        return borrowedBooks;
    }
}