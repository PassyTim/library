using Library.Application.Contracts;
using Library.Application.Services.BookUseCases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Attributes;

namespace Library.API.Controllers;

[ApiController]
[AutoValidation]
[Route("api/book")]
[ProducesResponseType(StatusCodes.Status401Unauthorized)]
public class BookController(
    GetAllBooksUseCase getAllBooksUseCase,
    GetBookByIdUseCase getBookByIdUseCase,
    GetBookByIsbnUseCase getBookByIsbnUseCase,
    CreateBookUseCase createBookUseCase,
    UpdateBookUseCase updateBookUseCase,
    RemoveBookUseCase removeBookUseCase
    ) : ControllerBase
{
    [Authorize]
    [HttpGet(Name = "GetAllBooks")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> GetAllBooks(int pageSize = 0, int pageNumber = 1)
    {
        Pagination pagination = new Pagination { PageSize = pageSize, PageNumber = pageNumber };
        SetPaginationHeader(pagination);

        var books = await getAllBooksUseCase.ExecuteAsync(pageSize:pageSize, pageNumber:pageNumber);
        var allBooks = await getAllBooksUseCase.ExecuteAsync();
        Response.Headers.Append("x-count", allBooks.Count.ToString());
        
        return Ok(books);
    }
    private void SetPaginationHeader(Pagination pagination)
    {
        Response.Headers.Append("Pagination", JsonConvert.SerializeObject(pagination));
    }

    [Authorize]
    [HttpGet("{id:int}", Name = "GetBookById")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> GetById(int id)
    {
        var book = await getBookByIdUseCase.ExecuteAsync(id);
        return Ok(book);
    }

    [Authorize]
    [HttpGet("{isbn}", Name = "GetBookByIsbn")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> GetByIsbn(string isbn)
    {
        var book = await getBookByIsbnUseCase.ExecuteAsync(isbn);
        return Ok(book);
    }

    [Authorize(Policy = "AdminPolicy")]
    [HttpPost(Name = "CreateBook")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult> Create([AutoValidateAlways][FromForm] BookRequest bookCreateRequest)
    {
        await createBookUseCase.ExecuteAsync(bookCreateRequest);
        return Ok();
    }
    
    [Authorize(Policy = "AdminPolicy")]
    [HttpPut("{id:int}",Name = "UpdateBook")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Update(int id,[AutoValidateAlways][FromForm] BookRequest bookUpdateRequest)
    {
        await updateBookUseCase.ExecuteAsync(id, bookUpdateRequest);
        return NoContent();
    }

    [Authorize(Policy = "AdminPolicy")]
    [HttpDelete("{id:int}", Name = "DeleteBook")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Remove(int id)
    {
        await removeBookUseCase.ExecuteAsync(id);
        return NoContent();
    }
}