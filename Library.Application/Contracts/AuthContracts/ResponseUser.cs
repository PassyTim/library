namespace Library.Application.Contracts.AuthContracts;

public record ResponseUser(
    string Id,
    string UserName,
    string Email);