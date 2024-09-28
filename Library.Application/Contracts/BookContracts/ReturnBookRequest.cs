namespace Library.Application.Contracts.BookContracts;

public record ReturnBookRequest(
    string UserId,
    int BookId);