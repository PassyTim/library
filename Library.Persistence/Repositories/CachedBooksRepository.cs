using System.Linq.Expressions;
using Library.Domain.IRepositories;
using Library.Domain.Models;
using Microsoft.Extensions.Caching.Memory;

namespace Library.Persistence.Repositories;

public class CachedBooksRepository(
    BooksRepository decorated,
    IMemoryCache memoryCache) : IBooksRepository
{
    public async Task<List<Book>> GetAllAsync(Expression<Func<Book, bool>>? filter = null, int pageSize = 0, int pageNumber = 0)
    {
        string key = $"all-books-{pageSize}-{pageNumber}";

        return (await memoryCache.GetOrCreateAsync(
            key,
            entry =>
            {
                entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(2));
                return decorated.GetAllAsync(filter, pageSize, pageNumber);
            }))!;
    }

    public async Task<Book?> GetById(int id)
    {
        string key = $"book-{id}";

        return (await memoryCache.GetOrCreateAsync(
            key,
            entry =>
            {
                entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(2));
                return decorated.GetById(id);
            }))!;
    }

    public async Task<Book?> GetByIsbn(string isbn)
    {
        string key = $"book-{isbn}";

        return (await memoryCache.GetOrCreateAsync(
            key,
            entry =>
            {
                entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(2));
                return decorated.GetByIsbn(isbn);
            }))!;
    }

    public async Task CreateAsync(Book book) => await decorated.CreateAsync(book);
    
    public async Task UpdateAsync(Book book) => await decorated.UpdateAsync(book);

    public async Task RemoveAsync(int bookId) => await decorated.RemoveAsync(bookId);

    public async Task<bool> IsBookWithIdExists(int id) => await decorated.IsBookWithIdExists(id);

    public async Task<bool> IsIsbnUnique(string isbn) => await decorated.IsIsbnUnique(isbn);

    public async Task<bool> IsIsbnUniqueForUpdate(string isbn, int id) =>
        await decorated.IsIsbnUniqueForUpdate(isbn, id);
}