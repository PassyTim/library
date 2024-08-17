using AutoMapper;
using Library.Application.Contracts;
using Library.Application.IServices;
using Library.Domain.IRepositories;
using Library.Domain.Models;

namespace Library.Application.Services;

public class AuthorService(IAuthorsRepository repository,
    IMapper mapper) : IAuthorService
{
    public async Task<List<AuthorResponse>> GetAll()
    {
        var authors = await repository.GetAllAsync();
        var authorResponseList = mapper.Map<List<AuthorResponse>>(authors);
        return authorResponseList;
    }

    public async Task<AuthorResponse> GetById(int id)
    {
        var author = await repository.GetById(id);
        var authorResponse = mapper.Map<AuthorResponse>(author);
        return authorResponse;
    }

    public async Task<AuthorResponse> GetByIdWithBooks(int id)
    {
        var author = await repository.GetByIdWithBooks(id);
        var authorResponse = mapper.Map<AuthorResponse>(author);
        return authorResponse;
    }

    public async Task Create(AuthorRequest authorCreateRequest)
    {
        var authorToCreate = mapper.Map<Author>(authorCreateRequest);
        await repository.CreateAsync(authorToCreate);
    }

    public async Task Update(AuthorRequest authorUpdateRequest)
    {
        var authorToUpdate = mapper.Map<Author>(authorUpdateRequest);
        await repository.UpdateAsync(authorToUpdate);
    }

    public async Task Remove(int id)
    {
        await repository.RemoveAsync(id);
    }
}