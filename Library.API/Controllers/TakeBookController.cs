using Library.Application.Contracts;
using Library.Application.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Library.API.Controllers;

[ApiController]
[Route("/api/takeBookService")]
public class TakeBookController(
    IBookService bookService) : ControllerBase
{
    [Authorize]
    [HttpPost("take")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> TakeBook([FromBody]BookTakeRequest bookTakeRequest)
    {
        await bookService.TakeBook(bookTakeRequest);
        return Ok();
    }

    [Authorize]
    [HttpDelete("return")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Return([FromBody] ReturnBookRequest returnBookRequest)
    {
        await bookService.ReturnBook(returnBookRequest);
        return NoContent();
    }

    [Authorize]
    [HttpGet("{userId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> GetAll(string userId)
    {
        var books = await bookService.GetTakenBooksByUserId(userId);
        return Ok(books);
    }
}