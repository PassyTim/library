using AutoMapper;
using FluentValidation;
using Library.Application.Contracts;
using Library.Application.Exceptions;
using Library.Domain.Models;
using Library.Persistence;
using Microsoft.AspNetCore.Identity;

namespace Library.Application.Services.UserUseCases;

public class RegisterUserUseCase(
    UserManager<User> userManager,
    RoleManager<IdentityRole> roleManager,
    IUnitOfWork unitOfWork,
    IMapper mapper)
{
    public async Task ExecuteAsync(UserRegisterRequest registerRequest)
    {
        await ValidateUserRegisterRequestAsync(registerRequest);
        
        var userToCreate = mapper.Map<User>(registerRequest);
        var result = await userManager.CreateAsync(userToCreate, registerRequest.Password);
        
        if (!result.Succeeded)
        {
            var errorMessages = result.Errors.Select(e => e.Description).ToList();
            throw new RegistrationException(string.Join(" ", errorMessages));
        }

        await AssignRolesAsync(registerRequest.Role, userToCreate);

        var registeredUser = await unitOfWork.UsersRepository.GetByEmail(registerRequest.Email);
        if (registeredUser == null)
        {
            throw new RegistrationException("Error registering user");
        }
    }

    private async Task ValidateUserRegisterRequestAsync(UserRegisterRequest request)
    {
        if (!IsValidEmail(request.Email))
        {
            throw new ValidationException("Invalid email");
        }

        if (!await IsUserUniqueAsync(request.Email))
        {
            throw new RegistrationException("This email is already in use");
        }
    }

    private bool IsValidEmail(string email)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }
    
    private async Task<bool> IsUserUniqueAsync(string email)
    {
        var user = await unitOfWork.UsersRepository.GetByEmail(email);
        return user == null;
    }

    private async Task AssignRolesAsync(string role, User user)
    {
        if (!await roleManager.RoleExistsAsync("Admin"))
        {
            await roleManager.CreateAsync(new IdentityRole("Admin"));
            await roleManager.CreateAsync(new IdentityRole("User"));
        }

        var roleToAssign = role == "Admin" ? "Admin" : "User";
        await userManager.AddToRoleAsync(user, roleToAssign);
    }
}