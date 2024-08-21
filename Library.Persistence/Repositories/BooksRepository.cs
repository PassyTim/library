using Library.Domain.IRepositories;
using Library.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Library.Persistence.Repositories;

public class BooksRepository(ApplicationDbContext dbContext) : IBooksRepository
{
    public async Task<List<Book>> GetAllAsync()
    {
        return await dbContext.Books
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Book?> GetById(int id)
    {
        return await dbContext.Books
            .AsNoTracking()
            .FirstOrDefaultAsync(b => b.Id == id);
    }

    public async Task<Book?> GetByIsbn(string isbn)
    {
        return await dbContext.Books
            .AsNoTracking()
            .FirstOrDefaultAsync(b => b.Isbn == isbn);
    }

    public async Task CreateAsync(Book book)
    {
        await dbContext.Books.AddAsync(book);
    }

    public async Task UpdateAsync(Book book)
    {
        await dbContext.Books
            .Where(b => b.Id == book.Id)
            .ExecuteUpdateAsync(s =>
                s.SetProperty(b => b.Description, book.Description)
                    .SetProperty(b=>b.Isbn, book.Isbn)
                    .SetProperty(b => b.Name, book.Name)
                    .SetProperty(b => b.Genre, book.Genre)
                    .SetProperty(b => b.ImageUrl, book.ImageUrl)
                    .SetProperty(b => b.AuthorId, book.AuthorId));
        
    }

    public async Task RemoveAsync(int bookId)
    {
        await dbContext.Books
            .Where(b => b.Id == bookId)
            .ExecuteDeleteAsync();
    }

    public async Task<bool> IsBookWithIdExists(int id)
    {
        return await dbContext.Books.AnyAsync(b => b.Id == id);
    }

    public async Task<bool> IsIsbnUnique(string isbn)
    {
        return !await dbContext.Books.AnyAsync(b => b.Isbn == isbn);
    }

    public async Task<bool> IsIsbnUniqueForUpdate(string isbn, int id)
    {
        return !await dbContext.Books.AnyAsync(b => b.Isbn == isbn && b.Id != id);
    }
}