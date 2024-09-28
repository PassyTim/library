namespace Library.Application.Contracts.AuthContracts;

public record UserLoginRequest(
    string Email,
    string Password
);