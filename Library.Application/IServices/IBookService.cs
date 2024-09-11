using System.Linq.Expressions;
using Library.Application.Contracts;
using Library.Domain.Models;

namespace Library.Application.IServices;

public interface IBookService
{
    public Task<List<BookResponse>> GetAll(int pageSize = 0, int pageNumber = 0);
    public Task<BookResponse> GetById(int id);
    public Task<BookResponse> GetByIsbn(string isbn);
    public Task TakeBook(BookTakeRequest bookTakeRequest);
    public Task ReturnBook(ReturnBookRequest bookReturnRequest);
    public Task<List<BookResponse>> GetTakenBooksByUserId(string userId);
    public Task Create(BookRequest book);
    public Task Update(int id, BookRequest bookUpdate);
    public Task Remove(int id);
}