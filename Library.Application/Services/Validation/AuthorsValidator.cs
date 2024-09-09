using FluentValidation;
using FluentValidation.Results;
using Library.Application.Contracts;
using Library.Domain;
using Library.Domain.IRepositories;
using Library.Persistence;

namespace Library.Application.Services.Validation;

public class AuthorsValidator : AbstractValidator<AuthorRequest>
{
    protected override void RaiseValidationException(ValidationContext<AuthorRequest> context, ValidationResult result)
    {
        var ex = new ValidationException(result.Errors);
        throw new ArgumentException(ex.Message, ex);
    }

    public AuthorsValidator(IUnitOfWork unitOfWork)
    {
        RuleFor(a => a.Id)
            .CustomAsync(async (id, context, _) =>
            {
                if (context.RootContextData.TryGetValue("IsCreate", out var _))
                {
                    if (id != 0)
                    {
                        context.AddFailure("Id must be 0 while creating author");
                    }
                }
            })
            .CustomAsync(async (id, context, _) =>
            {
                if (context.RootContextData.TryGetValue("IsUpdate", out var _))
                {
                    var author = await unitOfWork.AuthorsRepository.GetById(id);
                    if (author is null)
                    {
                        context.AddFailure("Id",$"There is no author to update with id: {id}");
                    }

                    if (context.RootContextData.TryGetValue("Id", out var contextId))
                    {
                        if((int)contextId != id) context.AddFailure("Id","The id's must match");
                    }
                }
            });

        RuleFor(a => a.FirstName)
            .MaximumLength(Constants.AuthorFirstNameMaxLength)
            .WithMessage($"Author FirstName must be less than {Constants.AuthorFirstNameMaxLength} symbols");
        
        RuleFor(a => a.LastName)
            .MaximumLength(Constants.AuthorLastNameMaxLength)
            .WithMessage($"Author LastName must be less than {Constants.AuthorLastNameMaxLength} symbols");
        
        RuleFor(a => a.Country)
            .MaximumLength(Constants.AuthorCountryMaxLength)
            .WithMessage($"Author Country must be less than {Constants.AuthorCountryMaxLength} symbols");
    }
}