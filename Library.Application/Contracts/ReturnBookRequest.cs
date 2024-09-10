namespace Library.Application.Contracts;

public record ReturnBookRequest(
    string UserId,
    int BookId);