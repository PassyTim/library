using AutoMapper;
using Library.Application.Contracts;
using Library.Application.Contracts.BookContracts;
using Library.Application.Exceptions;
using Library.Persistence.UnitOfWork;

namespace Library.Application.Services.BookUseCases;

public class GetUserTakenBooksUseCase(
    IMapper mapper,
    IUnitOfWork unitOfWork)
{
    public async Task<List<BookResponse>> ExecuteAsync(string userId)
    {
        var books = await unitOfWork.BooksRepository.GetAllAsync(b => b.UserId == userId && b.UserId != null);
        if (books is null)
        {
            throw new ItemNotFoundException("Books not found");
        }

        return mapper.Map<List<BookResponse>>(books);
    }
}