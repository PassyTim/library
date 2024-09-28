using FluentValidation;
using FluentValidation.Results;
using Library.Application.Contracts;
using Library.Application.Contracts.BookContracts;
using Library.Domain;
using Library.Persistence.UnitOfWork;

namespace Library.Application.Services.Validation;

public class BooksValidator : AbstractValidator<BookRequest>
{
    protected override void RaiseValidationException(ValidationContext<BookRequest> context, ValidationResult result)
    {
        var ex = new ValidationException(result.Errors);
        throw new ArgumentException(ex.Message, ex);
    }

    public BooksValidator(IUnitOfWork unitOfWork)
    {
        RuleFor(b => b.Isbn)
            .MustAsync(async (isbn, _) => IsbnValidator.Validate(isbn))
            .WithMessage("Incorrect ISBN");

        RuleFor(b => b.AuthorId)
            .MustAsync(async (authorId, _) => authorId > 0).WithMessage("AuthorId must be greater than 0")
            .MustAsync(async (authorId, _) =>
            {
                var author = await unitOfWork.AuthorsRepository.GetById(authorId);
                return author is not null;
            }).WithMessage("There is no author with this id");

        RuleFor(b => b.Image)
            .MustAsync(async (image, _) =>
            {
                if (image is null) return true;
                List<string> validExtensions = [".jpg", ".jpeg", ".png"];
                var extension = Path.GetExtension(image.FileName);
                return validExtensions.Contains(extension);
            }).WithMessage("Invalid file extension")
            .MustAsync(async (image, _) =>
            {
                if (image is null) return true;
                var size = image.Length;
                return size < 5 * 1024 * 1024;
            }).WithMessage("Maximum file size is 5 mb");
        
        RuleFor(b => b.Name)
            .MaximumLength(Constants.BookNameMaxLength)
            .WithMessage($"Book name must be less than {Constants.BookNameMaxLength} symbols");
        
        RuleFor(b => b.Genre)
            .MaximumLength(Constants.BookGenreMaxLength)
            .WithMessage($"Book genre must be less than {Constants.BookGenreMaxLength} symbols");
    }
}