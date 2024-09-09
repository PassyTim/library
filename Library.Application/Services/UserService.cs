using System.Security.Cryptography;
using AutoMapper;
using Library.Application.Contracts;
using Library.Domain.Models;
using Library.Infrastructure.JwtProvider;
using Library.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Library.Application.Services;

public class UserService(
    IUnitOfWork unitOfWork,
    RoleManager<IdentityRole> roleManager,
    UserManager<User> userManager,
    IJwtProvider jwtProvider
)
{

    public async Task<bool> IsUserUniqueAsync(string email)
    {
        var user = await unitOfWork.UsersRepository.GetByEmail(email);
        return user is null;
    }

    public async Task<RegisterResponse> RegisterAsync(UserRegisterRequest userRegisterRequest)
    {
        User userToCreate = new()
        {
            UserName = userRegisterRequest.UserName,
            Email = userRegisterRequest.Email,
            NormalizedEmail = userRegisterRequest.Email.ToUpper()
        };
        var response = new RegisterResponse();

        try
        {
            var createResult = await userManager.CreateAsync(userToCreate, userRegisterRequest.Password);
            if (createResult.Succeeded)
            {
                if (!roleManager.RoleExistsAsync("Admin").GetAwaiter().GetResult())
                {
                    await roleManager.CreateAsync(new IdentityRole("Admin"));
                    await roleManager.CreateAsync(new IdentityRole("User"));
                }

                if (userRegisterRequest.Role == "Admin")
                {
                    await userManager.AddToRoleAsync(userToCreate, "Admin");
                }
                else
                {
                    await userManager.AddToRoleAsync(userToCreate, "User");
                }

                var userToReturn = await unitOfWork.UsersRepository.GetByEmail(userRegisterRequest.Email);
                if (userToReturn is not null)
                {
                    response.IsSuccess = true;
                }

                return response;
            }

            foreach (var item in createResult.Errors)
            {
                response.Errors.Add(item.Description);
            }
        }
        catch (Exception ex)
        {
            response.IsSuccess = false;
            response.Errors = [ex.Message];
            return response;
        }

        response.IsSuccess = false;
        return response;
    }

    public async Task<LoginResponse> LoginAsync(UserLoginRequest userLoginRequest, bool populateExp)
    {
        var responseUser = await userManager.Users
            .Where(u => u.Email == userLoginRequest.Email)
            .Select(u => new ResponseUser
            {
                Id = u.Id,
                UserName = u.UserName,
                Email = u.Email,
                BorrowedBooks = u.BorrowedBooks.Select(b => new Book
                {
                    Id = b.Id,
                    UserId = b.UserId,
                    TakeDate = b.TakeDate,
                    ReturnDate = b.ReturnDate
                }).ToList()
            })
            .SingleOrDefaultAsync();

        var user = await userManager.FindByEmailAsync(userLoginRequest.Email);
            
        if (user is null)
        {
            return new LoginResponse { User = null, AccessToken = "", RefreshToken = "" };
        }

        bool isPasswordValid = await userManager.CheckPasswordAsync(user, userLoginRequest.Password);
        if (!isPasswordValid) return new LoginResponse { User = null, AccessToken = "", RefreshToken = "" };

        var tokens = await CreateToken(user, populateExp);
        
        LoginResponse response = new LoginResponse
        {
            User = responseUser,
            AccessToken = tokens.AccessToken,
            RefreshToken = tokens.RefreshToken
        };
        
        return response;
    }

    private async Task<Tokens> CreateToken(User user, bool populateExp)
    {
        var roles = await userManager.GetRolesAsync(user);
        var refreshToken = GenerateRefreshToken();
        string token = jwtProvider.Generate(user, roles);

        user.RefreshToken = refreshToken;
        if (populateExp)
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

        var tokensToReturn = new Tokens()
        {
            AccessToken = token,
            RefreshToken = refreshToken
        };
        await userManager.UpdateAsync(user);
        
        return tokensToReturn;
    }

    public async Task<Tokens> RefreshToken(string refreshToken)
    {
        var user = await userManager.Users.FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);
        if (user is null || user.RefreshToken != refreshToken ||
            user.RefreshTokenExpiryTime <= DateTime.Now)
        {
            throw new Exception("Invalid token");
        }

        return await CreateToken(user, false);
    }
    
    private string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }
}