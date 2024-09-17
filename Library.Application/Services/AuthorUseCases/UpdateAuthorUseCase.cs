using AutoMapper;
using FluentValidation;
using Library.Application.Contracts;
using Library.Domain.Models;
using Library.Persistence;

namespace Library.Application.Services.AuthorUseCases;

public class UpdateAuthorUseCase(
    IUnitOfWork unitOfWork,
    IMapper mapper)
{
    public async Task ExecuteAsync(AuthorRequest authorUpdateRequest, int id)
    {
        var author = await unitOfWork.AuthorsRepository.GetById(authorUpdateRequest.Id);
        if (author is null)
        {
            throw new ValidationException($"There is no author to update with id: {id}");
        }

        if (authorUpdateRequest.Id != id)
        {
            throw new ValidationException("The id's must match");
        }
        
        var authorToUpdate = mapper.Map<Author>(authorUpdateRequest);
        await unitOfWork.AuthorsRepository.UpdateAsync(authorToUpdate);
        await unitOfWork.SaveChangesAsync();
    }
}