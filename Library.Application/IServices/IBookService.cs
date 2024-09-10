using System.Linq.Expressions;
using Library.Application.Contracts;
using Library.Domain.Models;

namespace Library.Application.IServices;

public interface IBookService
{
    public Task<List<BookResponse>> GetAll(Expression<Func<Book, bool>>? filter = null,
        int pageSize = 0, int pageNumber = 0);
    public Task<BookResponse> GetById(int id);
    public Task<BookResponse> GetByIsbn(string isbn);
    public Task TakeBookUseCase(BookTakeRequest bookTakeRequest);
    public Task ReturnBookUseCase(ReturnBookRequest bookReturnRequest);
    public Task Create(BookRequest book);
    public Task Update(int id, BookRequest bookUpdate);
    public Task Remove(int id);
}