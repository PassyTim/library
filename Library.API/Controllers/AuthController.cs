using System.Net;
using Library.Application.Contracts;
using Library.Application.Contracts.AuthContracts;
using Library.Application.Services;
using Library.Application.Services.UserUseCases;
using Microsoft.AspNetCore.Mvc;

namespace Library.API.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(
    RegisterUserUseCase registerUserUseCase,
    LoginUserUseCase loginUserUseCase) : ControllerBase
{
    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> Register([FromBody]UserRegisterRequest userRegisterRequest)
    {
        await registerUserUseCase.ExecuteAsync(userRegisterRequest);
        return Ok();
    }

    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> Login([FromBody]UserLoginRequest userLoginRequest)
    {
        var loginResponse = await loginUserUseCase.ExecuteAsync(userLoginRequest, true);
        
        HttpContext.Response.Headers["Authorization"] = loginResponse.AccessToken;
        HttpContext.Response.Cookies.Append("refreshToken", loginResponse.RefreshToken, new CookieOptions
        {
            Expires = DateTimeOffset.UtcNow.AddDays(7),
            HttpOnly = true,
            IsEssential = true,
            Secure = true,
            SameSite = SameSiteMode.None
        });
        
        return Ok(loginResponse.User);
    }
}
