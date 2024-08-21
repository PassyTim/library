using System.Linq.Expressions;
using AutoMapper;
using Library.Application.Contracts;
using Library.Application.IServices;
using Library.Domain.Models;
using Library.Persistence;

namespace Library.Application.Services;

public class BookService(
    IUnitOfWork unitOfWork,
    IMapper mapper) : IBookService
{
    public async Task<List<BookResponse>> GetAll(Expression<Func<Book, bool>>? filter = null,
        int pageSize = 0, int pageNumber = 0)
    {
        var books = await unitOfWork.BooksRepository.GetAllAsync(filter, pageSize, pageNumber);
        var booksResponse = mapper.Map<List<BookResponse>>(books);
        return booksResponse;
    }

    public async Task<BookResponse> GetById(int id)
    {
        var book = await unitOfWork.BooksRepository.GetById(id);
        var bookResponse = mapper.Map<BookResponse>(book);
        
        return bookResponse;
    }

    public async Task<BookResponse> GetByIsbn(string isbn)
    {
        var book = await unitOfWork.BooksRepository.GetByIsbn(isbn);
        var bookResponse = mapper.Map<BookResponse>(book);

        return bookResponse;
    }
    

    public async Task Create(BookRequest bookCreate)
    {
        var bookToCreate = mapper.Map<Book>(bookCreate);
        var isbn = bookToCreate.Isbn;

        bookToCreate.Isbn = IsbnNormalizer.NormalizeIsbn(isbn);
        await unitOfWork.BooksRepository.CreateAsync(bookToCreate);
        await unitOfWork.SaveChangesAsync();
    }

    public async Task Update(BookRequest bookUpdate)
    {
        var bookToUpdate = mapper.Map<Book>(bookUpdate);
        var isbn = bookToUpdate.Isbn;

        bookToUpdate.Isbn = IsbnNormalizer.NormalizeIsbn(isbn);
        await unitOfWork.BooksRepository.UpdateAsync(bookToUpdate);
        await unitOfWork.SaveChangesAsync();
    }

    public async Task Remove(int id)
    {
        await unitOfWork.BooksRepository.RemoveAsync(id);
    }
}