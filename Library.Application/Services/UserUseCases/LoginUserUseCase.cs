using System.Security.Cryptography;
using Library.Application.Contracts;
using Library.Application.Exceptions;
using Library.Domain.Models;
using Library.Infrastructure;
using Library.Infrastructure.JwtProvider;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Library.Application.Services.UserUseCases;

public class LoginUserUseCase(
    UserManager<User> userManager,
    IJwtProvider jwtProvider)
{
    public async Task<LoginResponse> ExecuteAsync(UserLoginRequest request, bool populateExp)
    {
        var user = await userManager.Users
            .Include(u => u.BorrowedBooks)
            .SingleOrDefaultAsync(u => u.Email == request.Email);

        await ValidateLoginAsync(user, request);

        var tokens = await CreateTokensAsync(user, populateExp);
        
        return new LoginResponse
        {
            User = MapToResponseUser(user),
            AccessToken = tokens.AccessToken,
            RefreshToken = tokens.RefreshToken
        };
    }

    private async Task ValidateLoginAsync(User user, UserLoginRequest request)
    {
        if (user == null)
        {
            throw new LoginException("Incorrect email");
        }

        var isPasswordValid = await userManager.CheckPasswordAsync(user, request.Password);
        if (!isPasswordValid)
        {
            throw new LoginException("Invalid password");
        }
    }

    private async Task<Tokens> CreateTokensAsync(User user, bool populateExp)
    {
        var roles = await userManager.GetRolesAsync(user);
        var refreshToken = RefreshTokenGenerator.GenerateRefreshToken();
        var accessToken = jwtProvider.Generate(user, roles);

        user.RefreshToken = refreshToken;
        if (populateExp)
        {
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
        }

        await userManager.UpdateAsync(user);

        return new Tokens
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };
    }
    
    private ResponseUser MapToResponseUser(User user)
    {
        return new ResponseUser
        {
            Id = user.Id,
            Email = user.Email,
            UserName = user.UserName,
            BorrowedBooks = user.BorrowedBooks.Select(b => new Book
            {
                Id = b.Id,
                TakeDate = b.TakeDate,
                ReturnDate = b.ReturnDate
            }).ToList()
        };
    }
}