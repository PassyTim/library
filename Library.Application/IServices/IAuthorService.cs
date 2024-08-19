using Library.Application.Contracts;

namespace Library.Application.IServices;

public interface IAuthorService
{
    public Task<List<AuthorResponse>> GetAll();
    public Task<AuthorResponse> GetById(int id, bool isWithBooks = false);
    public Task Create(AuthorRequest authorCreateRequest);
    public Task Update(AuthorRequest authorUpdateRequest);
    public Task Remove(int id);
}