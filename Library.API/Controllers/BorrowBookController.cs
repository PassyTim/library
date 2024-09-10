using System.Net;
using FluentValidation;
using Library.Application.Contracts;
using Library.Application.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Library.API.Controllers;

[ApiController]
[Route("/api/takeBookService")]
public class BorrowBookController(
    IBookService bookService) : ControllerBase
{
    [Authorize]
    [HttpPost("take")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> TakeBook([FromBody]BookTakeRequest bookTakeRequest)
    {
        await bookService.TakeBookUseCase(bookTakeRequest);
        return Ok();
    }

    [Authorize]
    [HttpDelete("return")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Return([FromBody] ReturnBookRequest returnBookRequest)
    {
        await bookService.ReturnBookUseCase(returnBookRequest);
        return NoContent();
    }

    // [Authorize]
    // [HttpGet("{userId}")]
    // public async Task<ActionResult<ApiResponse>> GetAll(string userId)
    // {
    //     var books = await borrowBookService.GetAllByUserIdAsync(userId);
    //     if (books is null)
    //     {
    //         _response.StatusCode = HttpStatusCode.NotFound;
    //         _response.IsSuccess = false;
    //         _response.Errors.Add("No borrowed books to return");
    //
    //         return NotFound(_response);
    //     }
    //
    //     _response.Data = books;
    //     _response.IsSuccess = true;
    //     _response.StatusCode = HttpStatusCode.OK;
    //     return Ok(_response);
    // }
}