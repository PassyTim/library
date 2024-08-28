using System.Net;
using FluentValidation;
using Library.Application;
using Library.Application.Contracts;
using Library.Application.IServices;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Library.API.Controllers;

[ApiController]
[Route("api/book")]
public class BookController(IBookService bookService,
    IValidator<BookRequest> validator, 
    IConfiguration configuration) : ControllerBase
{
    private readonly ApiResponse _response = new();
    private readonly string _baseUrl = configuration["ImageBaseUrl"]!;
    
    [HttpGet(Name = "GetAllBooks")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse>> GetAllBooks(int pageSize = 0, int pageNumber = 1)
    {
        Pagination pagination = new Pagination { PageSize = pageSize, PageNumber = pageNumber };
        Response.Headers.Append("x-pagination", JsonConvert.SerializeObject(pagination));

        var allBooks = await bookService.GetAll();
        var books = await bookService.GetAll(pageSize:pageSize, pageNumber:pageNumber);
        foreach (var item in books)
        {
            var imagePath = item.ImageUrl;
            item.ImageUrl = _baseUrl + imagePath;
        }

        Response.Headers.Append("x-count", allBooks.Count.ToString());
            
        _response.Data = books;
        _response.StatusCode = HttpStatusCode.OK;
        return Ok(_response);
    }

    [HttpGet("{id:int}", Name = "GetBookById")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse>> GetById(int id)
    {
        var book = await bookService.GetById(id);
        var imagePath = book.ImageUrl;
        book.ImageUrl = _baseUrl + imagePath;
        
        _response.Data = book;
        
        if (_response.Data is null)
        {
            _response.Errors.Add("There is no book with this Id");
            _response.IsSuccess = false;
            _response.StatusCode = HttpStatusCode.NotFound;
            return NotFound(_response);
        }
        
        if (!_response.IsSuccess)
        {
            _response.StatusCode = HttpStatusCode.BadRequest;
            return BadRequest(_response);
        }

        _response.StatusCode = HttpStatusCode.OK;
        return Ok(_response);
    }

    [HttpGet("{isbn}", Name = "GetBookByIsbn")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse>> GetByIsbn(string isbn)
    {
        var book = await bookService.GetByIsbn(isbn);
        var imagePath = book.ImageUrl;
        book.ImageUrl = _baseUrl + imagePath;
        
        _response.Data = book;
        if (!IsbnValidator.Validate(isbn))
        {
            _response.Errors.Add("ISBN is not valid");
            _response.IsSuccess = false;
            _response.StatusCode = HttpStatusCode.BadRequest;
            return BadRequest(_response);
        }

        if (_response.Data is null)
        {
            _response.Errors.Add("There is no book with this ISBN");
            _response.IsSuccess = false;
            _response.StatusCode = HttpStatusCode.NotFound;
            return NotFound(_response);
        }

        _response.StatusCode = HttpStatusCode.OK;
        return Ok(_response);
    }

    [HttpPost(Name = "CreateBook")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse>> Create([FromForm] BookRequest bookCreateRequest)
    {
        var context = new ValidationContext<BookRequest>(bookCreateRequest);
        context.RootContextData["IsCreate"] = true;
        var validationResult = await validator.ValidateAsync(context);
        if (!validationResult.IsValid)
        {
            foreach (var item in validationResult.Errors)
            {
                _response.Errors.Add(item.ErrorMessage);
            }
            _response.IsSuccess = false;
        }
        
        if (!_response.IsSuccess)
        {
            _response.StatusCode = HttpStatusCode.BadRequest;
            return BadRequest(_response);
        }
        
        await bookService.Create(bookCreateRequest);
        _response.StatusCode = HttpStatusCode.Created;
        return Ok(_response);
    }
    
    [HttpPut("{id:int}",Name = "UpdateBook")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse>> Update(int id,[FromForm] BookRequest bookUpdateRequest)
    { 
        var context = new ValidationContext<BookRequest>(bookUpdateRequest);
        context.RootContextData["Id"] = id;
        context.RootContextData["IsUpdate"] = true;
        var validationResult = await validator.ValidateAsync(context);
        
        if (!validationResult.IsValid)
        {
            foreach (var item in validationResult.Errors)
            {
                _response.Errors.Add(item.ErrorMessage);
            }
            _response.IsSuccess = false;
        }
        
        if (!_response.IsSuccess)
        {
            _response.StatusCode = HttpStatusCode.BadRequest;
            return BadRequest(_response);
        }
        
        await bookService.Update(bookUpdateRequest);
        _response.StatusCode= HttpStatusCode.NoContent;
        return Ok(_response);
    }

    [HttpDelete("{id:int}", Name = "DeleteBook")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse>> Delete(int id)
    {
        var book = await bookService.GetById(id);
        if (book is null)
        {
            _response.IsSuccess = false;
            _response.Errors.Add($"There is no book to delete with id: {id}");
            _response.StatusCode = HttpStatusCode.BadRequest;
            return BadRequest(_response);
        }
        
        await bookService.Remove(id);
        
        if (!_response.IsSuccess)
        {
            _response.StatusCode = HttpStatusCode.BadRequest;
            return BadRequest(_response);
        }

        _response.StatusCode = HttpStatusCode.NoContent;
        return Ok(_response);
    }
}