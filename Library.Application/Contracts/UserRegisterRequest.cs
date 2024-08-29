using System.ComponentModel.DataAnnotations;

namespace Library.Application.Contracts;

public record UserRegisterRequest(
    [Required] string UserName,
    [Required] string Email,
    [Required] string Password,
    [Required] string Role = "User"
    );