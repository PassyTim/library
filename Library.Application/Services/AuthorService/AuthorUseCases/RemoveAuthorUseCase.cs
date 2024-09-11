using Library.Application.Exceptions;
using Library.Persistence;

namespace Library.Application.Services.AuthorService.AuthorUseCases;

public class RemoveAuthorUseCase(IUnitOfWork unitOfWork)
{
    public async Task ExecuteAsync(int id)
    {
        var author = await unitOfWork.AuthorsRepository.GetById(id);
        if (author is null)
        {
            throw new ItemNotFoundException($"Author with id:{id} not found");
        }
        
        await unitOfWork.AuthorsRepository.RemoveAsync(id);
    }
}