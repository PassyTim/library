using AutoMapper;
using Library.Application.Contracts;
using Library.Application.IServices;
using Library.Domain.IRepositories;
using Library.Domain.Models;

namespace Library.Application.Services;

public class BookService(IBooksRepository repository,
    IMapper mapper) : IBookService
{
    public async Task<List<BookResponse>> GetAll()
    {
        var books= await repository.GetAllAsync();
        var booksResponse = mapper.Map<List<BookResponse>>(books);
        return booksResponse;
    }

    public async Task<BookResponse> GetById(int id)
    {
        var book = await repository.GetById(id);
        var bookResponse = mapper.Map<BookResponse>(book);
        
        return bookResponse;
    }

    public async Task<BookResponse> GetByIsbn(string isbn)
    {
        var book = await repository.GetByIsbn(isbn);
        var bookResponse = mapper.Map<BookResponse>(book);

        return bookResponse;
    }
    

    public async Task Create(BookRequest bookCreate)
    {
        var bookToCreate = mapper.Map<Book>(bookCreate);
        var isbn = bookToCreate.Isbn;

        bookToCreate.Isbn = IsbnNormalizer.NormalizeIsbn(isbn);
        await repository.CreateAsync(bookToCreate);
    }

    public async Task Update(BookRequest bookUpdate)
    {
        var bookToUpdate = mapper.Map<Book>(bookUpdate);
        var isbn = bookToUpdate.Isbn;

        bookToUpdate.Isbn = IsbnNormalizer.NormalizeIsbn(isbn);
        await repository.UpdateAsync(bookToUpdate);
    }

    public async Task Remove(int id)
    {
        await repository.RemoveAsync(id);
    }
}