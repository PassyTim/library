using System.Security.Cryptography;
using AutoMapper;
using FluentValidation;
using Library.Application.Contracts;
using Library.Application.Exceptions;
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
    IJwtProvider jwtProvider,
    IMapper mapper
)
{
    public async Task RegisterAsync(UserRegisterRequest userRegisterRequest)
    {
        await ValidateUserRegisterRequestAsync(userRegisterRequest);
        
        User userToCreate = mapper.Map<User>(userRegisterRequest);
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

            var registeredUser = await unitOfWork.UsersRepository.GetByEmail(userRegisterRequest.Email);
            if (registeredUser is null)
            {
                throw new RegistrationException("Error registering user");
            }
        }
        else
        {
            var identityErrors = createResult.Errors.ToList();
            throw new RegistrationException(string.Join(" ", 
                identityErrors.Select(item => item.Description).ToList()));
        }
    }

    private async Task ValidateUserRegisterRequestAsync(UserRegisterRequest userRegisterRequest)
    {
        if (!IsValidEmail(userRegisterRequest.Email))
        {
            throw new ValidationException("Email is not valid");
        }
        
        bool isUserUnique = await IsUserUniqueAsync(userRegisterRequest.Email);
        if (!isUserUnique)
        {
            throw new RegistrationException("This email is already used");
        }
    }
    
    private async Task<bool> IsUserUniqueAsync(string email)
    {
        var user = await unitOfWork.UsersRepository.GetByEmail(email);
        return user is null;
    }
    private bool IsValidEmail(string email)
    {
        var trimmedEmail = email.Trim();

        if (trimmedEmail.EndsWith(".")) 
        {
            return false; 
        }
        try 
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == trimmedEmail;
        }
        catch {
            return false;
        }
    }

    public async Task<LoginResponse> LoginAsync(UserLoginRequest userLoginRequest, bool populateExp)
    {
        var user = await userManager.Users.Include(u=>u.BorrowedBooks)
            .SingleAsync(u=>u.Email == userLoginRequest.Email);
    
        await ValidateLoginAsync(user, userLoginRequest);

        var tokens = await CreateTokens(user, populateExp);
        
        LoginResponse response = new LoginResponse
        {
            User = CreateResponseUser(user)!,
            AccessToken = tokens.AccessToken,
            RefreshToken = tokens.RefreshToken
        };
        
        return response;
    }

    private async Task ValidateLoginAsync(User user, UserLoginRequest userLoginRequest)
    {
        if (user is null)
        {
            throw new LoginException("Incorrect email");
        }

        var isPasswordValid = await userManager.CheckPasswordAsync(user, userLoginRequest.Password);
        if (!isPasswordValid)
        {
            throw new LoginException("Invalid password");
        }
    }

    private ResponseUser? CreateResponseUser(User user)
    {
        return new ResponseUser
        {
            Id = user.Id,
            Email = user.Email,
            UserName = user.UserName,
            BorrowedBooks = user.BorrowedBooks.Select(b => new Book
            {
                Id = b.Id,
                UserId = b.UserId,
                TakeDate = b.TakeDate,
                ReturnDate = b.ReturnDate
            }).ToList()
        };
    }

    private async Task<Tokens> CreateTokens(User user, bool populateExp)
    {
        var roles = await userManager.GetRolesAsync(user);
        var refreshToken = GenerateRefreshToken();
        string accessToken = jwtProvider.Generate(user, roles);

        user.RefreshToken = refreshToken;
        if (populateExp)
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

        var tokensToReturn = new Tokens
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };
        
        await userManager.UpdateAsync(user);
        return tokensToReturn;
    }
    
    private static string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    public async Task<Tokens> RefreshToken(string? refreshToken)
    {
        if (refreshToken is null)
        {
            throw new TokenException("Invalid refresh token");
        }
        var user = await userManager.Users.FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);
        if (user is null || user.RefreshToken != refreshToken ||
            user.RefreshTokenExpiryTime <= DateTime.Now)
        {
            throw new TokenException("Invalid refresh token");
        }

        return await CreateTokens(user, false);
    }
}