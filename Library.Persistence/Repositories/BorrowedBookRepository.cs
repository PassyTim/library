using Library.Domain.IRepositories;
using Library.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Library.Persistence.Repositories;

public class BorrowedBookRepository(ApplicationDbContext dbContext) : IBorrowedBookRepository
{
    public async Task<BorrowedBook?> GetByIdAsync(int id)
    {
        return await dbContext.BorrowedBooks
            .AsNoTracking()
            .FirstOrDefaultAsync(b => b.Id == id);
    }

    public async Task<List<BorrowedBook>> GetAllByUserId(string userId)
    {
        return await dbContext.BorrowedBooks
            .Where(b => b.UserId == userId)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task RemoveAsync(int id)
    {
        await dbContext.BorrowedBooks
            .Where(b => b.Id == id)
            .ExecuteDeleteAsync();
    }

    public async Task CreateAsync(BorrowedBook borrowedBook)
    {
        await dbContext.BorrowedBooks.AddAsync(borrowedBook);
    }

    public async Task<bool> IsBorrowedBookExistAsync(int id)
    {
        return await dbContext.BorrowedBooks.AnyAsync(b => b.Id == id);
    }
}