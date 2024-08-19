using FluentValidation;
using Library.Application.Contracts;
using Library.Domain;

namespace Library.Application.Services;

public class AuthorValidator : AbstractValidator<AuthorRequest>
{
    public AuthorValidator()
    {
        RuleFor(a => a.Id)
            .MustAsync(async (id, _) => id > 0).WithMessage("Id must be greater than 0");

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