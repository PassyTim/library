using System.Text.RegularExpressions;
using FluentValidation;
using Library.Application.Contracts;
using Library.Domain.IRepositories;
using Library.Domain.Models;

namespace Library.Application.Services;

public class BooksValidator : AbstractValidator<BookRequest>
{
    public BooksValidator(IBooksRepository booksRepository)
    {
        RuleFor(b => b.Isbn).MustAsync( async (isbn, _) =>
        {
            string pattern =
                "^(?=(?:\\D*\\d){10}(?:(?:\\D*\\d){3})?$)[\\d-]+$";
            Regex regex = new Regex(pattern);
            var matches = regex.Matches(isbn);
            
            if (matches.Count > 0) return true;
            return false;
        }).WithMessage("Incorrect ISBN");
        
        RuleFor(b => b.Isbn).MustAsync(async (isbn, _) =>
        {
            return await booksRepository.IsIsbnUnique(isbn);
        }).WithMessage("ISBN must be unique");
    }
}