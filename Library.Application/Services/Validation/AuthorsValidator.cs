using FluentValidation;
using FluentValidation.Results;
using Library.Application.Contracts;
using Library.Application.Contracts.AuthorContracts;
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

    public AuthorsValidator()
    {
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