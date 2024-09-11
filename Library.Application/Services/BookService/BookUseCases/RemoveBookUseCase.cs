using Library.Application.Exceptions;
using Library.Persistence;

namespace Library.Application.Services.BookService.BookUseCases;

public class RemoveBookUseCase(
    IUnitOfWork unitOfWork)
{
    public async Task ExecuteAsync(int id)
    {
        var book = await unitOfWork.BooksRepository.GetById(id);
        if (book is null)
        {
            throw new ItemNotFoundException($"Book with id:{id} not found");
        }
        
        await unitOfWork.BooksRepository.RemoveAsync(id);
    }
}