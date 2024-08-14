using Library.Domain.Models;

namespace Library.Domain.IRepositories;

public interface IBookRepository
{
    Task<List<Book>> GetAllAsync();
    Task<Book> GetById(int id);
    Task<Book> GetByIsbn(string isbn);
    Task CreateAsync(Book book);
    Task UpdateAsync(Book book);
    Task RemoveAsync(Book book);
    Task SaveAsync();
}