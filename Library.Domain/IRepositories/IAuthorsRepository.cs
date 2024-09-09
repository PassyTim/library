using System.Linq.Expressions;
using Library.Domain.Models;

namespace Library.Domain.IRepositories;

public interface IAuthorsRepository
{
    Task<List<Author>> GetAllAsync(Expression<Func<Author, bool>>? filter = null, int pageSize = 0, int pageNumber = 0);
    Task<Author?> GetById(int id);
    Task CreateAsync(Author author);
    Task UpdateAsync(Author author);
    Task RemoveAsync(int authorId);
}