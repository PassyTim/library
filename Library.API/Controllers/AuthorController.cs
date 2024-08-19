using System.Net;
using FluentValidation;
using Library.Application.Contracts;
using Library.Application.IServices;
using Microsoft.AspNetCore.Mvc;

namespace Library.API.Controllers;

[ApiController]
[Route("api/author")]
public class AuthorController(
    IAuthorService service,
    IValidator<AuthorRequest> validator) : ControllerBase
{
    private readonly ApiResponse _response = new();
    
    [HttpGet(Name = "GetAllAuthors")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse>> GetAll()
    {
        _response.Data = await service.GetAll();
        _response.StatusCode = HttpStatusCode.OK;
        return Ok(_response);
    }

    [HttpGet("{id:int}/{isWithBooks:bool}", Name = "GetAuthorById")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse>> GetById(int id, bool isWithBooks)
    {
        if (id <= 0)
        {
            _response.Errors.Add("Incorrect Id");
            _response.IsSuccess = false;
            _response.StatusCode = HttpStatusCode.BadRequest;
            return BadRequest(_response);
        }
        
        _response.Data = await service.GetById(id, isWithBooks);
        if (!_response.IsSuccess)
        {
            _response.StatusCode = HttpStatusCode.NotFound;
            return BadRequest(_response);
        }
        
        if (_response.Data is null)
        {
            _response.Errors.Add("There is no author with this Id");
            _response.IsSuccess = false;
            _response.StatusCode = HttpStatusCode.NotFound;
            return NotFound(_response);
        }
        
        _response.StatusCode = HttpStatusCode.OK;
        return Ok(_response);
    }

    [HttpPost(Name = "CreateAuthor")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse>> Create([FromBody] AuthorRequest authorCreateRequest)
    {
        if (authorCreateRequest.Id != 0)
        {
            _response.StatusCode = HttpStatusCode.BadRequest;
            _response.Errors.Add("Id must be 0 while creating author");
            _response.IsSuccess = false;
            return BadRequest(_response);
        }
        
        if (!_response.IsSuccess )
        {
            _response.StatusCode = HttpStatusCode.BadRequest;
            _response.IsSuccess = false;
            return BadRequest(_response);
        }
        
        await service.Create(authorCreateRequest);
        _response.StatusCode = HttpStatusCode.Created;
        return Ok(_response);
    }

    [HttpPut("{id:int}", Name = "UpdateAuthor")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse>> Update(int id, [FromBody] AuthorRequest authorUpdateRequest)
    {
        var validationResult = await validator.ValidateAsync(authorUpdateRequest);
        var author = await service.GetById(id);
        if (author is null)
        {
            _response.IsSuccess = false;
            _response.Errors.Add($"There is no author to update with id: {id}");
            _response.StatusCode = HttpStatusCode.BadRequest;
            return BadRequest(_response);
        }
        
        if (!_response.IsSuccess || id != authorUpdateRequest.Id)
        {
            _response.StatusCode = HttpStatusCode.BadRequest;
            _response.IsSuccess = false;
            return BadRequest(_response);
        }
        
        await service.Update(authorUpdateRequest);
        _response.StatusCode = HttpStatusCode.NoContent;
        return Ok(_response);
    }
    
    [HttpDelete("{id:int}", Name = "DeleteAuthor")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse>> Delete(int id)
    {
        var author = await service.GetById(id);
        if (author is null)
        {
            _response.IsSuccess = false;
            _response.Errors.Add($"There is no author to delete with id: {id}");
            _response.StatusCode = HttpStatusCode.BadRequest;
            return BadRequest(_response);
        }
        
        await service.Remove(id);
        
        if (!_response.IsSuccess)
        {
            _response.StatusCode = HttpStatusCode.BadRequest;
            return BadRequest(_response);
        }
        
        _response.StatusCode = HttpStatusCode.NoContent;
        return Ok(_response);
    }
}