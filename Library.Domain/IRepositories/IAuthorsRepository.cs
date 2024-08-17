using Library.Domain.Models;

namespace Library.Domain.IRepositories;

public interface IAuthorsRepository
{
    Task<List<Author>> GetAllAsync();
    Task<Author?> GetById(int id);
    Task<Author?> GetByIdWithBooks(int id);
    Task CreateAsync(Author author);
    Task UpdateAsync(Author author);
    Task RemoveAsync(int authorId);
    Task<bool> IsAuthorWithIdExists(int id);
    Task SaveAsync();
}