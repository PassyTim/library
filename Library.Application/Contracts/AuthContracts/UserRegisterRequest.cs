namespace Library.Application.Contracts.AuthContracts;

public record UserRegisterRequest(
    string UserName,
    string Email,
    string Password,
    string Role = "User"
    );