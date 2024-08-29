using System.ComponentModel.DataAnnotations;

namespace Library.Application.Contracts;

public record UserLoginRequest(
    [Required] string Email,
    [Required] string Password
);