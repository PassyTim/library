using System.Net;
using FluentValidation;
using Library.Application;
using Library.Application.Contracts;
using Library.Application.IServices;
using Microsoft.AspNetCore.Mvc;

namespace Library.API.Controllers;

[ApiController]
[Route("api/book")]
public class BookController(IBookService bookService,
    IAuthorService authorService,
    IValidator<BookRequest> validator) : ControllerBase
{
    private readonly ApiResponse _response = new();
    
    [HttpGet(Name = "GetAllBooks")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse>> GetAllBooks()
    {
        _response.Data = await bookService.GetAll();
        _response.StatusCode = HttpStatusCode.OK;
        return Ok(_response);
    }

    [HttpGet("{id:int}", Name = "GetBookById")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse>> GetById(int id)
    {
        _response.Data = await bookService.GetById(id);
        if (!_response.IsSuccess)
        {
            _response.StatusCode = HttpStatusCode.NotFound;
            return BadRequest(_response);
        }
        
        if (_response.Data is null)
        {
            _response.Errors.Add("There is no book with this Id");
            _response.IsSuccess = false;
            _response.StatusCode = HttpStatusCode.NotFound;
            return NotFound(_response);
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
        _response.Data = await bookService.GetByIsbn(isbn);
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
    public async Task<ActionResult<ApiResponse>> Create([FromBody] BookRequest bookCreateRequest)
    {
        var author = await authorService.GetById(bookCreateRequest.AuthorId);
        if (author is null)
        {
            _response.StatusCode = HttpStatusCode.BadRequest;
            _response.Errors.Add("Invalid author id");
            _response.IsSuccess = false;
            return BadRequest(_response);
        }
        
        var validationResult = await validator.ValidateAsync(bookCreateRequest);
        if (!validationResult.IsValid)
        {
            foreach (var item in validationResult.Errors)
            {
                _response.Errors.Add(item.ErrorMessage);
            }
            _response.IsSuccess = false;
        }

        if (bookCreateRequest.Id != 0)
        {
            _response.StatusCode = HttpStatusCode.BadRequest;
            _response.Errors.Add("Id must be 0 while creating book");
            _response.IsSuccess = false;
            return BadRequest(_response);
        }
        
        if (!_response.IsSuccess )
        {
            _response.StatusCode = HttpStatusCode.BadRequest;
            _response.IsSuccess = false;
            return BadRequest(_response);
        }
        
        await bookService.Create(bookCreateRequest);
        _response.StatusCode = HttpStatusCode.Created;
        return Ok(_response);
    }

    [HttpDelete("{id:int}", Name = "DeleteBook")]
    [ProducesResponseType(StatusCodes.Status200OK)]
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

        _response.StatusCode = HttpStatusCode.OK;
        return Ok(_response);
    }

    [HttpPut("{id:int}",Name = "UpdateBook")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse>> Update(int id,[FromBody] BookRequest bookUpdateRequest)
    { 
        var author = await authorService.GetById(bookUpdateRequest.AuthorId);
        if (author is null)
        {
            _response.StatusCode = HttpStatusCode.BadRequest;
            _response.Errors.Add("Invalid author id");
            _response.IsSuccess = false;
            return BadRequest(_response);
        }
        
        var validationResult = await validator.ValidateAsync(bookUpdateRequest);
        var book = await bookService.GetById(id);
        
        if (book is null)
        {
            _response.IsSuccess = false;
            _response.Errors.Add($"There is no book to update with id: {id}");
            _response.StatusCode = HttpStatusCode.BadRequest;
            return BadRequest(_response);
        }
        
        if (!validationResult.IsValid && book.Isbn != bookUpdateRequest.Isbn)
        {
            foreach (var item in validationResult.Errors)
            {
                _response.Errors.Add(item.ErrorMessage);
            }
            _response.IsSuccess = false;
        }
        
        if (!_response.IsSuccess || id != bookUpdateRequest.Id)
        {
            _response.StatusCode = HttpStatusCode.BadRequest;
            _response.IsSuccess = false;
            return BadRequest(_response);
        }
        
        await bookService.Update(bookUpdateRequest);
        _response.StatusCode= HttpStatusCode.OK;
        return Ok(_response);
    }
}