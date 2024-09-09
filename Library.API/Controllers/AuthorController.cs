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
        
        var authorsResponse = await service.GetAll(pageSize:pageSize, pageNumber:pageNumber);
        return Ok(authorsResponse);
    }

    [Authorize]
    [HttpGet("{id:int}", Name = "GetAuthorById")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<AuthorResponse>> GetById(int id)
    {
        var authorResponse = await service.GetById(id);
        return Ok(authorResponse);
    }

    [Authorize(Policy = "AdminPolicy")]
    [HttpPost(Name = "CreateAuthor")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Create([FromBody] AuthorRequest authorCreateRequest)
    {
        await service.Create(authorCreateRequest);
        return Ok();
    }

    [Authorize(Policy = "AdminPolicy")]
    [HttpPut("{id:int}", Name = "UpdateAuthor")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Update(int id, [FromBody] AuthorRequest authorUpdateRequest)
    {
        await service.Update(authorUpdateRequest);
        return NoContent();
    }
    
    [Authorize(Policy = "AdminPolicy")]
    [HttpDelete("{id:int}", Name = "DeleteAuthor")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Delete(int id)
    {
        await service.Remove(id);
        return NoContent();
    }
}