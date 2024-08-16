using System.Net;
using FluentValidation;
using Library.Application.Contracts;
using Library.Application.IServices;
using Microsoft.AspNetCore.Mvc;

namespace Library.API.Controllers;

[ApiController]
[Route("api/book")]
public class BookController(IBookService bookService,
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

        _response.StatusCode = HttpStatusCode.OK;
        return Ok(_response);
    }

    [HttpPost(Name = "CreateBook")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse>> Create([FromBody] BookRequest bookCreateRequest)
    {
        var validationResult = await validator.ValidateAsync(bookCreateRequest);
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

    [HttpDelete("{id:int}", Name = "DeleteBook")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse>> Delete(int id)
    {
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
    public async Task<ActionResult<ApiResponse>> Update([FromBody] BookRequest bookUpdateRequest)
    { 
        // var validationResult = await validator.ValidateAsync(bookUpdateRequest);
        // if (!validationResult.IsValid)
        // {
        //     foreach (var item in validationResult.Errors)
        //     {
        //         _response.Errors.Add(item.ErrorMessage);
        //     }
        //     _response.IsSuccess = false;
        // }
        
        if (!_response.IsSuccess)
        {
            _response.StatusCode = HttpStatusCode.BadRequest;
            return BadRequest(_response);
        }
        
        await bookService.Update(bookUpdateRequest);
        _response.StatusCode= HttpStatusCode.OK;
        return Ok(_response);
    }
}