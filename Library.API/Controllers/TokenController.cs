using Library.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Library.API.Controllers;

[ApiController]
[Route("api/token")]
public class TokenController(UserService service) : ControllerBase
{
    [HttpPost("refresh")]
    public async Task<ActionResult<string>> Refresh()
    {
        HttpContext.Request.Cookies.TryGetValue("refreshToken", out var refreshToken);
        
        var tokensToReturn = await service.RefreshToken(refreshToken);
        
        HttpContext.Response.Cookies.Append("refreshToken", tokensToReturn.RefreshToken, new CookieOptions
        {
            Expires = DateTimeOffset.UtcNow.AddDays(7),
            HttpOnly = true,
            IsEssential = true,
            Secure = true,
            SameSite = SameSiteMode.None
        });
        
        return Ok(tokensToReturn.AccessToken);
    }
}