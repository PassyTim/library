using Library.Application.Contracts;
using Library.Application.IServices;
using Library.Application.Services.AuthorService.AuthorUseCases;

namespace Library.Application.Services.AuthorService;

public class AuthorService(
    GetAllAuthorsUseCase getAllAuthorsUseCase,
    GetAuthorByIdUseCase getAuthorByIdUseCase,
    CreateAuthorUseCase createAuthorUseCase,
    UpdateAuthorUseCase updateAuthorUseCase,
    RemoveAuthorUseCase removeAuthorUseCase) : IAuthorService
{
    public async Task<List<AuthorResponse>> GetAll(int pageSize, int pageNumber)
    {
        return await getAllAuthorsUseCase.ExecuteAsync(pageSize, pageNumber);
    }

    public async Task<AuthorResponse> GetById(int id)
    {
        return await getAuthorByIdUseCase.ExecuteAsync(id);
    }

    public async Task Create(AuthorRequest authorCreateRequest)
    {
        await createAuthorUseCase.ExecuteAsync(authorCreateRequest);
    }

    public async Task Update(AuthorRequest authorUpdateRequest, int id)
    {
        await updateAuthorUseCase.ExecuteAsync(authorUpdateRequest, id);
    }

    public async Task Remove(int id)
    {
        await removeAuthorUseCase.ExecuteAsync(id);
    }
}