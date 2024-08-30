using System.Security.Claims;
using Library.Domain.Models;

namespace Library.Infrastructure.JwtProvider;

public interface IJwtProvider
{
    string Generate(User user, IList<string> roles);
    ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
}