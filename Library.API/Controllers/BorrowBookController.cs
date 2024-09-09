// using System.Net;
// using FluentValidation;
// using Library.Application.Contracts;
// using Library.Application.IServices;
// using Microsoft.AspNetCore.Authorization;
// using Microsoft.AspNetCore.Mvc;
//
// namespace Library.API.Controllers;
//
// [ApiController]
// [Route("/api/borrowService")]
// public class BorrowBookController(
//     IBookService borrowBookService,
//     IValidator<BorrowBookRequest> validator) : ControllerBase
// {
//     private readonly ApiResponse _response = new();
//     
//     [Authorize]
//     [HttpPost("borrow")]
//     [ProducesResponseType(StatusCodes.Status200OK)]
//     [ProducesResponseType(StatusCodes.Status401Unauthorized)]
//     [ProducesResponseType(StatusCodes.Status400BadRequest)]
//     public async Task<IActionResult> Borrow([FromBody]BorrowBookRequest borrowBookRequest)
//     {
//         var validationResult = await validator.ValidateAsync(borrowBookRequest);
//         if (!validationResult.IsValid)
//         {
//             _response.StatusCode = HttpStatusCode.BadRequest;
//             _response.IsSuccess = false;
//             foreach (var item in validationResult.Errors)
//             {
//                 _response.Errors.Add(item.ErrorMessage);
//             }
//
//             return BadRequest(_response);
//         }
//         
//         await borrowBookService.BorrowBookAsync(borrowBookRequest);
//         _response.IsSuccess = true;
//         _response.StatusCode = HttpStatusCode.OK;
//         return Ok(_response);
//     }
//
//     [Authorize]
//     [HttpDelete("return/{id:int}")]
//     [ProducesResponseType(StatusCodes.Status200OK)]
//     [ProducesResponseType(StatusCodes.Status401Unauthorized)]
//     [ProducesResponseType(StatusCodes.Status400BadRequest)]
//     public async Task<IActionResult> Return(int id)
//     {
//         var result = await borrowBookService.TryReturnBookAsync(id);
//         if (!result)
//         {
//             _response.StatusCode = HttpStatusCode.BadRequest;
//             _response.IsSuccess = false;
//             _response.Errors.Add("No borrowed book with this id to return");
//
//             return BadRequest(_response);
//         }
//         
//         _response.IsSuccess = true;
//         _response.StatusCode = HttpStatusCode.OK;
//         return Ok(_response);
//     }
//
//     [Authorize]
//     [HttpGet("{userId}")]
//     public async Task<ActionResult<ApiResponse>> GetAll(string userId)
//     {
//         var books = await borrowBookService.GetAllByUserIdAsync(userId);
//         if (books is null)
//         {
//             _response.StatusCode = HttpStatusCode.NotFound;
//             _response.IsSuccess = false;
//             _response.Errors.Add("No borrowed books to return");
//
//             return NotFound(_response);
//         }
//
//         _response.Data = books;
//         _response.IsSuccess = true;
//         _response.StatusCode = HttpStatusCode.OK;
//         return Ok(_response);
//     }
// }