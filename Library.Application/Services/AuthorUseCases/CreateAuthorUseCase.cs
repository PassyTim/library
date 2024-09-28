using AutoMapper;
using FluentValidation;
using Library.Application.Contracts;
using Library.Application.Contracts.AuthorContracts;
using Library.Domain.Models;
using Library.Persistence.UnitOfWork;

namespace Library.Application.Services.AuthorUseCases;

public class CreateAuthorUseCase(
    IUnitOfWork unitOfWork,
    IMapper mapper)
{
    public async Task ExecuteAsync(AuthorRequest authorCreateRequest)
    {
        if (authorCreateRequest.Id != 0)
        {
            throw new ValidationException("Id must be 0 while creating a new author.");
        }
        
        var authorToCreate = mapper.Map<Author>(authorCreateRequest);
        await unitOfWork.AuthorsRepository.CreateAsync(authorToCreate);
        await unitOfWork.SaveChangesAsync();
    }
}