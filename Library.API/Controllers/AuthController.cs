using System.Net;
using Library.Application.Contracts;
using Library.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Library.API.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(UserService userService) : ControllerBase
{
    private readonly ApiResponse _apiResponse = new();
    
    [HttpPost("register")]
    public async Task<ActionResult<ApiResponse>> Register([FromBody]UserRegisterRequest userRegisterRequest)
    {
        bool isUserUnique = await userService.IsUserUniqueAsync(userRegisterRequest.Email);
        if (!isUserUnique)
        {
            _apiResponse.IsSuccess = false;
            _apiResponse.StatusCode = HttpStatusCode.BadRequest;
            _apiResponse.Errors = ["This email is already used"];
            return BadRequest(_apiResponse);
        }

        var registrationResult = await userService.RegisterAsync(userRegisterRequest);
        if (registrationResult.IsSuccess)
        {
            _apiResponse.StatusCode = HttpStatusCode.OK;
            return Ok(_apiResponse);
        }
        
        _apiResponse.IsSuccess = false;
        _apiResponse.StatusCode = HttpStatusCode.BadRequest;
        _apiResponse.Errors = registrationResult.Errors.ToList();
        return BadRequest(_apiResponse);
    }

    [HttpPost("login")]
    public async Task<ActionResult<ApiResponse>> Login([FromBody]UserLoginRequest userLoginRequest)
    {
        var loginResponse = await userService.LoginAsync(userLoginRequest, true);
        
        HttpContext.Response.Headers["Authorization"] = loginResponse.AccessToken;
        HttpContext.Response.Cookies.Append("refreshToken", loginResponse.RefreshToken, new CookieOptions
        {
            Expires = DateTimeOffset.UtcNow.AddDays(7),
            HttpOnly = true,
            IsEssential = true,
            Secure = true,
            SameSite = SameSiteMode.None
        });
        
        if (loginResponse.User is null || string.IsNullOrEmpty(loginResponse.AccessToken))
        {
            _apiResponse.IsSuccess = false;
            _apiResponse.Errors = ["Username or password is incorrect!"];
            _apiResponse.StatusCode = HttpStatusCode.BadRequest;
            return BadRequest(_apiResponse);
        }
        
        _apiResponse.StatusCode = HttpStatusCode.OK;
        _apiResponse.Data = loginResponse.User;
        return Ok(_apiResponse);
    }
}
