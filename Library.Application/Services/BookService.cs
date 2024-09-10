using System.Linq.Expressions;
using AutoMapper;
using FluentValidation;
using Library.Application.Contracts;
using Library.Application.Exceptions;
using Library.Application.IServices;
using Library.Domain.Models;
using Library.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace Library.Application.Services;

public class BookService(
    IUnitOfWork unitOfWork,
    IMapper mapper,
    IConfiguration configuration,
    IValidator<BookRequest> validator,
    UserManager<User> userManager) : IBookService
{
    private readonly string _baseUrl = configuration["ImageBaseUrl"]!;

    public async Task<List<BookResponse>> GetAll(Expression<Func<Book, bool>>? filter = null,
        int pageSize = 0, int pageNumber = 0)
    {
        if (pageSize > 100) pageSize = 100;
        
        var books = await unitOfWork.BooksRepository.GetAllAsync(filter, pageSize, pageNumber);
        var booksResponse = mapper.Map<List<BookResponse>>(books);
        foreach (var item in booksResponse)
        {
            var imagePath = item.ImageUrl;
            item.ImageUrl = _baseUrl + imagePath;
        }

        return booksResponse;
    }

    public async Task<BookResponse> GetById(int id)
    {
        var book = await unitOfWork.BooksRepository.GetById(id);
        
        if (book is null)
        {
            throw new ItemNotFoundException($"Book with id:{id} not found");
        }
        
        var bookResponse = mapper.Map<BookResponse>(book);
        var imagePath = bookResponse.ImageUrl;
        bookResponse.ImageUrl = _baseUrl + imagePath;
        
        return bookResponse;
    }

    public async Task<BookResponse> GetByIsbn(string isbn)
    {
        var book = await unitOfWork.BooksRepository.GetByIsbn(isbn);

        if (!IsbnValidator.Validate(isbn))
        {
            throw new ValidationException("Invalid ISBN");
        }
        if (book is null)
        {
            throw new ItemNotFoundException($"Book with isbn:{isbn} not found");
        }
        
        var bookResponse = mapper.Map<BookResponse>(book);
        var imagePath = bookResponse.ImageUrl;
        bookResponse.ImageUrl = _baseUrl + imagePath;
        
        return bookResponse;
    }

    public async Task Create(BookRequest bookCreate)
    {
        var context = new ValidationContext<BookRequest>(bookCreate);
        context.RootContextData["IsCreate"] = true;
        var validationResult = await validator.ValidateAsync(context);
        
        if (!validationResult.IsValid)
        {
            var errorMessages = validationResult.Errors.Select(x => x.ErrorMessage);
            throw new ValidationException(string.Join(". ", errorMessages));
        }
            
        FileUploadHandler fileUploadHandler = new FileUploadHandler();
        var path = fileUploadHandler.Upload(bookCreate.Image);
        
        var bookToCreate = mapper.Map<Book>(bookCreate);
        var isbn = bookToCreate.Isbn;

        bookToCreate.ImagePath = path;
        bookToCreate.Isbn = IsbnNormalizer.NormalizeIsbn(isbn);
        await unitOfWork.BooksRepository.CreateAsync(bookToCreate);
        await unitOfWork.SaveChangesAsync();
    }

    public async Task Update(int id, BookRequest bookUpdate)
    {
        var context = new ValidationContext<BookRequest>(bookUpdate);
        context.RootContextData["Id"] = id;
        context.RootContextData["IsUpdate"] = true;
        var validationResult = await validator.ValidateAsync(context);
        
        if (!validationResult.IsValid)
        {
            var errorMessages = validationResult.Errors.Select(x => x.ErrorMessage);
            throw new ValidationException(string.Join(". ", errorMessages));
        }
        
        FileUploadHandler fileUploadHandler = new FileUploadHandler();
        var path = fileUploadHandler.Upload(bookUpdate.Image);
        
        var bookToUpdate = mapper.Map<Book>(bookUpdate);
        var isbn = bookToUpdate.Isbn;

        bookToUpdate.ImagePath = path;
        bookToUpdate.Isbn = IsbnNormalizer.NormalizeIsbn(isbn);
        bookToUpdate.UserId = null;
        await unitOfWork.BooksRepository.UpdateAsync(bookToUpdate);
        await unitOfWork.SaveChangesAsync();
    }

    public async Task TakeBookUseCase(BookTakeRequest bookTakeRequest)
    {
        var book = await unitOfWork.BooksRepository.GetById(bookTakeRequest.BookId);
        var user = await userManager.FindByIdAsync(bookTakeRequest.UserId);

        if (book is null || user is null)
        {
            throw new ItemNotFoundException("Book or user not found");
        }

        book.ReturnDate = bookTakeRequest.ReturnDate;
        book.TakeDate = DateTime.Today;
        book.UserId = user.Id;

        await unitOfWork.BooksRepository.UpdateAsync(book);
        await unitOfWork.SaveChangesAsync();
    }

    public async Task ReturnBookUseCase(ReturnBookRequest returnBookRequest)
    {
        var book = await unitOfWork.BooksRepository.GetById(returnBookRequest.BookId);
        var user = await userManager.FindByIdAsync(returnBookRequest.UserId);

        if (book is null || user is null)
        {
            throw new ItemNotFoundException("Book or user not found");
        }

        book.ReturnDate = DateTime.Today;
        book.TakeDate = DateTime.Today;
        book.UserId = null;
        
        await unitOfWork.BooksRepository.UpdateAsync(book);
        await unitOfWork.SaveChangesAsync();
    }

    public async Task Remove(int id)
    {
        var book = await unitOfWork.BooksRepository.GetById(id);
        if (book is null)
        {
            throw new ItemNotFoundException($"Book with id:{id} not found");
        }
        
        await unitOfWork.BooksRepository.RemoveAsync(id);
    }
}