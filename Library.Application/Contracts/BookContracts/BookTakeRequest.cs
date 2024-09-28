namespace Library.Application.Contracts.BookContracts;

public record BookTakeRequest(
    string UserId,
    int BookId,
    DateTime ReturnDate);