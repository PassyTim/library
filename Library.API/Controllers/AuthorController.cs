using Library.Application.Contracts;
using Library.Application.Contracts.AuthorContracts;
using Library.Application.Services.AuthorUseCases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Attributes;

namespace Library.API.Controllers;

[ApiController]
[AutoValidation]
[Route("api/author")]
[ProducesResponseType(StatusCodes.Status401Unauthorized)]
public class AuthorController(
    GetAllAuthorsUseCase getAllAuthorsUseCase,
    GetAuthorByIdUseCase getAuthorByIdUseCase,
    CreateAuthorUseCase createAuthorUseCase,
    RemoveAuthorUseCase removeAuthorUseCase,
    UpdateAuthorUseCase updateAuthorUseCase) : ControllerBase
{
    [Authorize]
    [HttpGet(Name = "GetAllAuthors")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<AuthorResponse>>> GetAll(int pageSize = 0, int pageNumber = 1)
    {
        Pagination pagination = new Pagination { PageSize = pageSize, PageNumber = pageNumber };
        SetPaginationHeader(pagination);
        
        var authorsResponse = 
            await getAllAuthorsUseCase.ExecuteAsync(pageSize:pageSize, pageNumber:pageNumber);
        return Ok(authorsResponse);
    }
    private void SetPaginationHeader(Pagination pagination)
    {
        Response.Headers.Append("Pagination", JsonConvert.SerializeObject(pagination));
    }

    [Authorize]
    [HttpGet("{id:int}", Name = "GetAuthorById")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AuthorResponse>> GetById(int id)
    {
        var authorResponse = await getAuthorByIdUseCase.ExecuteAsync(id);
        return Ok(authorResponse);
    }

    [Authorize(Policy = "AdminPolicy")]
    [HttpPost(Name = "CreateAuthor")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Create([AutoValidateAlways][FromBody] AuthorRequest authorCreateRequest)
    {
        await createAuthorUseCase.ExecuteAsync(authorCreateRequest);
        return Ok();
    }

    [Authorize(Policy = "AdminPolicy")]
    [HttpPut("{id:int}", Name = "UpdateAuthor")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Update(int id, [AutoValidateAlways][FromBody] AuthorRequest authorUpdateRequest)
    {
        await updateAuthorUseCase.ExecuteAsync(authorUpdateRequest, id);
        return NoContent();
    }
    
    [Authorize(Policy = "AdminPolicy")]
    [HttpDelete("{id:int}", Name = "DeleteAuthor")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Remove(int id)
    {
        await removeAuthorUseCase.ExecuteAsync(id);
        return NoContent();
    }
}