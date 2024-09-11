using Library.Application.Contracts;
using Library.Application.IServices;
using Library.Application.Services.BookService.BookUseCases;

namespace Library.Application.Services.BookService;

public class BookService(
    GetBookByIdUseCase getBookByIdUseCase,
    GetBookByIsbnUseCase getBookByIsbnUseCase,
    GetAllBooksUseCase getAllBooksUseCase,
    CreateBookUseCase createBookUseCase,
    UpdateBookUseCase updateBookUseCase,
    RemoveBookUseCase removeBookUseCase,
    TakeBookUseCase takeBookUseCase,
    ReturnBookUseCase returnBookUseCase,
    GetUserTakenBooksUseCase getUserTakenBooksUseCase) : IBookService
{
    public async Task<List<BookResponse>> GetAll(int pageSize, int pageNumber)
    {
        return await getAllBooksUseCase.ExecuteAsync(pageSize, pageNumber);
    }

    public async Task<BookResponse> GetById(int id)
    {
        return await getBookByIdUseCase.ExecuteAsync(id);
    }

    public async Task<BookResponse> GetByIsbn(string isbn)
    {
        return await getBookByIsbnUseCase.ExecuteAsync(isbn);
    }

    public async Task Create(BookRequest bookCreate)
    {
        await createBookUseCase.ExecuteAsync(bookCreate);
    }

    public async Task Update(int id, BookRequest bookUpdate)
    {
        await updateBookUseCase.ExecuteAsync(id, bookUpdate);
    }
    
    public async Task Remove(int id)
    {
        await removeBookUseCase.ExecuteAsync(id);
    }

    public async Task TakeBook(BookTakeRequest bookTakeRequest)
    {
        await takeBookUseCase.ExecuteAsync(bookTakeRequest);
    }

    public async Task ReturnBook(ReturnBookRequest returnBookRequest)
    {
        await returnBookUseCase.ExecuteAsync(returnBookRequest);
    }

    public async Task<List<BookResponse>> GetTakenBooksByUserId(string userId)
    {
        return await getUserTakenBooksUseCase.ExecuteAsync(userId);
    }

    
}