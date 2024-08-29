using Library.Application.Contracts;
using Library.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Library.API.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(UserService userService) : ControllerBase
{
    [HttpPost("register")]
    public async Task<ActionResult<ApiResponse>> Register([FromBody]UserRegisterRequest userRegisterRequest)
    {
        bool isUserUnique = await userService.IsUserUniqueAsync(userRegisterRequest.Email);
        if (!isUserUnique) return BadRequest();
        
        if (await userService.TryRegisterAsync(userRegisterRequest))
        {
            return Ok();
        }
        
        return BadRequest();
    }

    [HttpPost("login")]
    public async Task<ActionResult<ApiResponse>> Login([FromBody]UserLoginRequest userLoginRequest)
    {
        var loginResponse = await userService.LoginAsync(userLoginRequest);
        
        HttpContext.Response.Headers.Authorization = loginResponse.Token;
        
        if (loginResponse.User is null || string.IsNullOrEmpty(loginResponse.Token))
        {
            return BadRequest();
        }
        
        return Ok(loginResponse);
    }
}