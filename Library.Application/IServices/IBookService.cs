using Library.Application.Contracts;

namespace Library.Application.IServices;

public interface IBookService
{
    public Task<List<BookResponse>> GetAll();
    public Task<BookResponse> GetById(int id);
    public Task Create(BookRequest book);
    public Task Update(BookRequest bookUpdate);
    public Task Remove(int bookId);
}