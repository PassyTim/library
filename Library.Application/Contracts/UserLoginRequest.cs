using System.ComponentModel.DataAnnotations;

namespace Library.Application.Contracts;

public record UserLoginRequest(
    string Email,
    string Password
);