using System.Linq.Expressions;
using Library.Domain.IRepositories;
using Library.Domain.Models;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace Library.Persistence.Repositories;

public class CachedBooksRepository(
    BooksRepository decorated,
    IDistributedCache distributedCache) : IBooksRepository
{
    private readonly int _cacheDurationInSeconds = 120;
    public async Task<List<Book>> GetAllAsync(Expression<Func<Book, bool>>? filter = null, int pageSize = 0, int pageNumber = 0)
    {
        if(filter is not null)
            return await decorated.GetAllAsync(filter, pageSize, pageNumber);
        
        string key = $"all-books-{pageSize}-{pageNumber}";
        string? cachedBooks = await distributedCache.GetStringAsync(key);
        List<Book>? books;
        
        if (string.IsNullOrEmpty(cachedBooks))
        {
            books = await decorated.GetAllAsync(filter, pageSize, pageNumber);
            if (books is null)
            {
                return books;
            }

            await distributedCache.SetStringAsync(key,
                JsonConvert.SerializeObject(books),
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(_cacheDurationInSeconds)
                });

            return books;
        }

        books = JsonConvert.DeserializeObject<List<Book>>(cachedBooks)!;
        return books;
    }

    public async Task<Book?> GetById(int id)
    {
        string key = $"book-{id}";

        string? cachedBook = await distributedCache.GetStringAsync(key);
        Book? book;

        if (string.IsNullOrEmpty(cachedBook))
        {
            book = await decorated.GetById(id);
            if (book is null)
            {
                return book;
            }

            await distributedCache.SetStringAsync(key,
                JsonConvert.SerializeObject(book),
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(_cacheDurationInSeconds)
                });
            return book;
        }
        
        book = JsonConvert.DeserializeObject<Book>(cachedBook)!;
        return book;
    }

    public async Task<Book?> GetByIsbn(string isbn)
    {
        string key = $"book-{isbn}";

        string? cachedBook = await distributedCache.GetStringAsync(key);
        Book? book;

        if (string.IsNullOrEmpty(cachedBook))
        {
            book = await decorated.GetByIsbn(isbn);
            if (book is null)
            {
                return book;
            }

            await distributedCache.SetStringAsync(key,
                JsonConvert.SerializeObject(book),
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(_cacheDurationInSeconds)
                });
            return book;
        }
        
        book = JsonConvert.DeserializeObject<Book>(cachedBook)!;
        return book;
    }

    public async Task CreateAsync(Book book) => await decorated.CreateAsync(book);

    public async Task UpdateAsync(Book book)
    {
        await decorated.UpdateAsync(book);
        
        string key = $"book-{book.Id}";
        await distributedCache.RemoveAsync(key);
    }

    public async Task RemoveAsync(int bookId)
    {
        await decorated.RemoveAsync(bookId);
        
        string key = $"book-{bookId}";
        await distributedCache.RemoveAsync(key);
    }
}