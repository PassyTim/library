using AutoMapper;
using Library.Application.Contracts;
using Library.Domain.Models;
using Library.Infrastructure;
using Library.Persistence;
using Microsoft.AspNetCore.Identity;

namespace Library.Application.Services;

public class UserService(
    IUnitOfWork unitOfWork, 
    IMapper mapper,
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
    
    public async Task<bool> TryRegisterAsync(UserRegisterRequest userRegisterRequest)
    {
        User userToCreate = new()
        {
            UserName = userRegisterRequest.UserName,
            Email = userRegisterRequest.Email,
            NormalizedEmail = userRegisterRequest.Email.ToUpper()
        };

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
                
                await userManager.AddToRoleAsync(userToCreate, "User");

                var userToReturn = await unitOfWork.UsersRepository.GetByEmail(userRegisterRequest.Email);
                return userToReturn is not null;
            }
        }
        catch (Exception ex)
        {
            return false;
        }

        return false;
    }

    public async Task<LoginResponse> LoginAsync(UserLoginRequest userLoginRequest)
    {
        var user = await unitOfWork.UsersRepository.GetByEmail(userLoginRequest.Email);
        if (user is null)
        {
            return new LoginResponse{User = null, Token = ""};
        }
        
        bool isPasswordValid = await userManager.CheckPasswordAsync(user, userLoginRequest.Password);
        if (!isPasswordValid) return new LoginResponse{User = null, Token = ""};
        
        var roles = await userManager.GetRolesAsync(user);
        string token = jwtProvider.Generate(user, roles);

        LoginResponse response = new LoginResponse
        {
            User = mapper.Map<ResponseUser>(user), 
            Token = token
        };

        return response;
    }
}