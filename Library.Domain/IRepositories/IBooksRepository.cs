using System.Linq.Expressions;
using Library.Domain.Models;

namespace Library.Domain.IRepositories;

public interface IBooksRepository
{
    Task<List<Book>> GetAllAsync(Expression<Func<Book, bool>>? filter = null, int pageSize = 0, int pageNumber = 0);
    Task<Book?> GetById(int id);
    Task<Book?> GetByIsbn(string isbn);
    Task CreateAsync(Book book);
    Task UpdateAsync(Book book);
    Task RemoveAsync(int bookId);
}