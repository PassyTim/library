using System.Linq.Expressions;
using Library.Application.Contracts;
using Library.Domain.Models;

namespace Library.Application.IServices;

public interface IAuthorService
{
    public Task<List<AuthorResponse>> GetAll(Expression<Func<Author, bool>>? filter = null,
        int pageSize = 0, int pageNumber = 0);
    public Task<AuthorResponse> GetById(int id);
    public Task Create(AuthorRequest authorCreateRequest);
    public Task Update(AuthorRequest authorUpdateRequest, int id);
    public Task Remove(int id);
}