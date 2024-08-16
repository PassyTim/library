using AutoMapper;
using FluentValidation;
using Library.Application.Contracts;
using Library.Application.IServices;
using Library.Domain.IRepositories;
using Library.Domain.Models;

namespace Library.Application.Services;

public class BookService(IBooksRepository repository,
    IMapper mapper,
    IValidator<BookRequest> validator) : IBookService
{
    public async Task<List<BookResponse>> GetAll()
    {
        var books= await repository.GetAllAsync();
        var booksReponse = mapper.Map<List<BookResponse?>>(books);
        return booksReponse;
    }

    public async Task<BookResponse> GetById(int id)
    {
        var book = await repository.GetById(id);
        var bookResponse = mapper.Map<BookResponse>(book);
        
        return bookResponse;
    }

    public async Task Create(BookRequest bookCreate)
    {
        var bookToCreate = mapper.Map<Book>(bookCreate);
        await repository.CreateAsync(bookToCreate);
    }

    public async Task Update(BookRequest bookUpdate)
    {
        var book = mapper.Map<Book>(bookUpdate);
        await repository.UpdateAsync(book);
    }

    public async Task Remove(int bookId)
    {
        var exists = await repository.IsBookWithIdExists(bookId);
        if (!exists)
        {
            throw new Exception();
        }
        
        await repository.RemoveAsync(bookId);
    }
}