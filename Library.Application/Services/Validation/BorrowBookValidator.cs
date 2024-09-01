using FluentValidation;
using Library.Application.Contracts;
using Library.Persistence;

namespace Library.Application.Services.Validation;

public class BorrowBookValidator : AbstractValidator<BorrowBookRequest>
{
    public BorrowBookValidator(IUnitOfWork unitOfWork)
    {
        RuleFor(b => b.ReturnDate)
            .MustAsync(async (returnDate, _) 
                => returnDate > DateTime.Today.AddDays(1) && returnDate < DateTime.Today.AddMonths(1))
            .WithMessage("Incorrect returnDate, you should return the book no sooner than one day and no later than one month later ");

        RuleFor(b => b.BookId)
            .MustAsync(async (bookId, _) =>
            {
                var book = await unitOfWork.BooksRepository.GetById(bookId);
                return book.AvailableCount > 0;
            })
            .WithMessage("There are no such books in the library");
    }
}