using System.Net;
using FluentValidation;
using Library.Application.Contracts;
using Library.Application.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Library.API.Controllers;

[ApiController]
[Route("api/author")]
public class AuthorController(
    IAuthorService service,
    IValidator<AuthorRequest> validator) : ControllerBase
{
    private readonly ApiResponse _response = new();
    
    [Authorize]
    [HttpGet(Name = "GetAllAuthors")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<ApiResponse>> GetAll(int pageSize = 0, int pageNumber = 1)
    {
        Pagination pagination = new Pagination { PageSize = pageSize, PageNumber = pageNumber };
        Response.Headers.Append("Pagination", JsonConvert.SerializeObject(pagination));
        
        _response.Data = await service.GetAll(pageSize:pageSize, pageNumber:pageNumber);
        _response.StatusCode = HttpStatusCode.OK;
        return Ok(_response);
    }

    [Authorize]
    [HttpGet("{id:int}/{isWithBooks:bool}", Name = "GetAuthorById")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<ApiResponse>> GetById(int id, bool isWithBooks)
    {
        _response.Data = await service.GetById(id, isWithBooks);
        
        if (_response.Data is null)
        {
            _response.Errors.Add("There is no author with this Id");
            _response.IsSuccess = false;
            _response.StatusCode = HttpStatusCode.NotFound;
            return NotFound(_response);
        }
        
        if (!_response.IsSuccess)
        {
            _response.StatusCode = HttpStatusCode.NotFound;
            return BadRequest(_response);
        }
        
        _response.StatusCode = HttpStatusCode.OK;
        return Ok(_response);
    }

    [Authorize(Policy = "AdminPolicy")]
    [HttpPost(Name = "CreateAuthor")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<ApiResponse>> Create([FromBody] AuthorRequest authorCreateRequest)
    {
        var validationContext = new ValidationContext<AuthorRequest>(authorCreateRequest);
        validationContext.RootContextData["IsCreate"] = true;
        var validationResult = await validator.ValidateAsync(validationContext);
        
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
        
        await service.Create(authorCreateRequest);
        _response.StatusCode = HttpStatusCode.Created;
        return Ok(_response);
    }

    [Authorize(Policy = "AdminPolicy")]
    [HttpPut("{id:int}", Name = "UpdateAuthor")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<ApiResponse>> Update(int id, [FromBody] AuthorRequest authorUpdateRequest)
    {
        var validationContext = new ValidationContext<AuthorRequest>(authorUpdateRequest);
        validationContext.RootContextData["IsUpdate"] = true;
        validationContext.RootContextData["Id"] = id;
        var validationResult = await validator.ValidateAsync(validationContext);
        
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
        
        await service.Update(authorUpdateRequest);
        _response.StatusCode = HttpStatusCode.NoContent;
        return Ok(_response);
    }
    
    [Authorize(Policy = "AdminPolicy")]
    [HttpDelete("{id:int}", Name = "DeleteAuthor")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
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