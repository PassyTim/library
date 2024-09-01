using FluentValidation;
using Library.Application.Contracts;
using Library.Domain;
using Library.Persistence;

namespace Library.Application.Services.Validation;

public class BooksValidator : AbstractValidator<BookRequest>
{
    public BooksValidator(IUnitOfWork unitOfWork)
    {
        RuleFor(b => b.Id).CustomAsync(async (id, context, _) =>
        {
            if (context.RootContextData.TryGetValue("Id", out var value))
            {
                var contextId = (int)value;
                if (contextId != id)
                {
                    context.AddFailure("Id", "The id's must match");
                }
            }
        }).CustomAsync(async (id, context, _) =>
        {
            if (context.RootContextData.TryGetValue("IsUpdate", out var _))
            {
                var book = await unitOfWork.BooksRepository.GetById(id);
                if (book is null)
                {
                    context.AddFailure("Id", $"There is no book to update with id {id}");
                }
            }
        })
        .CustomAsync(async (id, context, _) =>
        {
            if (context.RootContextData.TryGetValue("IsCreate", out var _))
            {
                if (id != 0)
                {
                    context.AddFailure("Id", "Id must be 0 while creating book");
                }
            }
        });
        
        RuleFor(b => b.Isbn)
            .MustAsync(async (isbn, _) => IsbnValidator.Validate(isbn)).WithMessage("Incorrect ISBN")
            .CustomAsync(async (isbn, context,_) =>
            {
                if (context.RootContextData.TryGetValue("IsUpdate", out var _))
                {
                    var id = (int)context.RootContextData["Id"];
                    var isIsbnUnique = await unitOfWork.BooksRepository.IsIsbnUniqueForUpdate(isbn, id);
                    if (!isIsbnUnique)
                    {
                        context.AddFailure("Isbn", "ISBN must be unique");
                    }
                }
            })
            .CustomAsync(async (isbn, context, _) =>
            {
                if (context.RootContextData.TryGetValue("IsCreate", out var _))
                {
                    var isIsbnUnique = await unitOfWork.BooksRepository.IsIsbnUnique(isbn);
                    if (!isIsbnUnique)
                    {
                        context.AddFailure("Isbn", "ISBN must be unique");
                    }
                }
            });

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

        RuleFor(b => b.AvailableCount)
            .MustAsync(async (availableCount, _) => availableCount >= 0)
            .WithMessage("Available count must be greater than 0 or 0");
        
        RuleFor(b => b.TotalCount)
            .MustAsync(async (totalCount, _) => totalCount >= 0)
            .WithMessage("Total count must be greater than 0 or 0");
    }
}