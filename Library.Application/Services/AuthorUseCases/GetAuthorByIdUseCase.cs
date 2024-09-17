using AutoMapper;
using Library.Application.Contracts;
using Library.Application.Exceptions;
using Library.Persistence;

namespace Library.Application.Services.AuthorUseCases;

public class GetAuthorByIdUseCase(
    IUnitOfWork unitOfWork,
    IMapper mapper)
{
    public async Task<AuthorResponse> ExecuteAsync(int id)
    {
        var author = await unitOfWork.AuthorsRepository.GetById(id);
        
        if (author is null)
        {
            throw new ItemNotFoundException($"Author with id:{id} not found");
        }
        
        var authorResponse = mapper.Map<AuthorResponse>(author);
        return authorResponse;
    }
}