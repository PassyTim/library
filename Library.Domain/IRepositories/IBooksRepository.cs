using Library.Domain.Models;

namespace Library.Domain.IRepositories;

public interface IBooksRepository
{
    Task<List<Book>> GetAllAsync();
    Task<Book?> GetById(int id);
    Task<Book?> GetByIsbn(string isbn);
    Task CreateAsync(Book book);
    Task UpdateAsync(Book book);
    Task RemoveAsync(int bookId);
    Task<bool> IsBookWithIdExists(int id);
    Task<bool> IsIsbnUnique(string isbn);
    Task<bool> IsIsbnUniqueForUpdate(string isbn, int id);
}