using FluentValidation;
using Library.Application.Contracts;
using Library.Domain.IRepositories;

namespace Library.Application.Services;

public class BooksValidator : AbstractValidator<BookRequest>
{
    public BooksValidator(IBooksRepository booksRepository)
    {
        RuleFor(b => b.Isbn).MustAsync( 
            async (isbn, _) => IsbnValidator.Validate(isbn)).WithMessage("Incorrect ISBN");
        
        RuleFor(b => b.Isbn).MustAsync(async (isbn, _) =>
        {
            return await booksRepository.IsIsbnUnique(isbn);
        }).WithMessage("ISBN must be unique");

        RuleFor(b => b.AuthorId).MustAsync(async (authorId, _) => 
            authorId > 0).WithMessage("AuthorId must be greater than 0");
    }
}