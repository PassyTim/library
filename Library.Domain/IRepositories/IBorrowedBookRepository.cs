using Library.Domain.Models;

namespace Library.Domain.IRepositories;

public interface IBorrowedBookRepository
{
    Task<BorrowedBook?> GetByIdAsync(int id);
    Task<List<BorrowedBook>> GetAllByUserId(string userId);
    Task RemoveAsync(int id);
    Task CreateAsync(BorrowedBook borrowedBook);
    Task<bool> IsBorrowedBookExistAsync(int id);
}