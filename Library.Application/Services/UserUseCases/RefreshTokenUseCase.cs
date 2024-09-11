using System.Security.Cryptography;
using Library.Application.Contracts;
using Library.Application.Exceptions;
using Library.Domain.Models;
using Library.Infrastructure;
using Library.Infrastructure.JwtProvider;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Library.Application.Services.UserUseCases;

public class RefreshTokenUseCase(
    UserManager<User> userManager,
    IJwtProvider jwtProvider)
{
    public async Task<Tokens> ExecuteAsync(string refreshToken)
    {
        var user = await userManager.Users.FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);

        if (user == null || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
        {
            throw new TokenException("Invalid or expired refresh token");
        }

        return await CreateTokensAsync(user);
    }

    private async Task<Tokens> CreateTokensAsync(User user)
    {
        var roles = await userManager.GetRolesAsync(user);
        var refreshToken = RefreshTokenGenerator.GenerateRefreshToken();
        var accessToken = jwtProvider.Generate(user, roles);

        user.RefreshToken = refreshToken;
        await userManager.UpdateAsync(user);

        return new Tokens
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };
    }
}