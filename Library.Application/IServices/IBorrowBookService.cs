using Library.Application.Contracts;
using Library.Domain.Models;

namespace Library.Application.IServices;

public interface IBorrowBookService
{
    public Task BorrowBookAsync(BorrowBookRequest borrowBookRequest);
    public Task<bool> TryReturnBookAsync(int id);
    public Task<List<BorrowedBook>> GetAllByUserIdAsync(string userId);
}