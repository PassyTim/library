using AutoMapper;
using FluentValidation;
using Library.Application.Contracts;
using Library.Domain.Models;
using Library.Persistence;

namespace Library.Application.Services.AuthorService.AuthorUseCases;

public class UpdateAuthorUseCase(
    IUnitOfWork unitOfWork,
    IMapper mapper,
    IValidator<AuthorRequest> validator)
{
    public async Task ExecuteAsync(AuthorRequest authorUpdateRequest, int id)
    {
        var validationContext = new ValidationContext<AuthorRequest>(authorUpdateRequest);
        validationContext.RootContextData["IsUpdate"] = true;
        validationContext.RootContextData["Id"] = id;
        var validationResult = await validator.ValidateAsync(validationContext);

        if (!validationResult.IsValid)
        {
            var errorMessages = validationResult.Errors.Select(x => x.ErrorMessage);
            throw new ValidationException(string.Join(". ", errorMessages));
        }
        
        var authorToUpdate = mapper.Map<Author>(authorUpdateRequest);
        await unitOfWork.AuthorsRepository.UpdateAsync(authorToUpdate);
        await unitOfWork.SaveChangesAsync();
    }
}