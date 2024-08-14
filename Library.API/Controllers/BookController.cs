using Microsoft.AspNetCore.Mvc;

namespace Library.API.Controllers;

[ApiController]
[Route("api/book")]
public class BookController : ControllerBase
{
    [HttpGet]
    public void GetAllBooks()
    {
        
    }
}