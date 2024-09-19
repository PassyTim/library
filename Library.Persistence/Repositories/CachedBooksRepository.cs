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
        if(filter is not null)
            return await decorated.GetAllAsync(filter, pageSize, pageNumber);
        
        string key = $"all-books-{pageSize}-{pageNumber}";

        return (await memoryCache.GetOrCreateAsync(
            key,
            entry =>
            {
                entry.SetAbsoluteExpiration(TimeSpan.FromSeconds(20));
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
                entry.SetAbsoluteExpiration(TimeSpan.FromSeconds(20));
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
                entry.SetAbsoluteExpiration(TimeSpan.FromSeconds(20));
                return decorated.GetByIsbn(isbn);
            }))!;
    }

    public async Task CreateAsync(Book book) => await decorated.CreateAsync(book);

    public async Task UpdateAsync(Book book)
    {
        await decorated.UpdateAsync(book);
        
        string key = $"book-{book.Id}";
        memoryCache.Remove(key);
    }

    public async Task RemoveAsync(int bookId)
    {
        await decorated.RemoveAsync(bookId);
        
        string key = $"book-{bookId}";
        memoryCache.Remove(key);
    }
}