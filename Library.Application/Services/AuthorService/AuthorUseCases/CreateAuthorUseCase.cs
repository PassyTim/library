using AutoMapper;
using FluentValidation;
using Library.Application.Contracts;
using Library.Domain.Models;
using Library.Persistence;

namespace Library.Application.Services.AuthorService.AuthorUseCases;

public class CreateAuthorUseCase(
    IUnitOfWork unitOfWork,
    IMapper mapper,
    IValidator<AuthorRequest> validator)
{
    public async Task ExecuteAsync(AuthorRequest authorCreateRequest)
    {
        var validationContext = new ValidationContext<AuthorRequest>(authorCreateRequest);
        validationContext.RootContextData["IsCreate"] = true;
        var validationResult = await validator.ValidateAsync(validationContext);
        
        if (!validationResult.IsValid)
        {
            var errorMessages = validationResult.Errors.Select(x => x.ErrorMessage);
            throw new ValidationException(string.Join(". ", errorMessages));
        }
        
        var authorToCreate = mapper.Map<Author>(authorCreateRequest);
        await unitOfWork.AuthorsRepository.CreateAsync(authorToCreate);
        await unitOfWork.SaveChangesAsync();
    }
}