using System.ComponentModel.DataAnnotations;

namespace Library.Application.Contracts;

public record UserRegisterRequest(
    string UserName,
    string Email,
    string Password,
    string Role = "User"
    );