namespace Library.Application.Contracts;

public record BookTakeRequest(
    string UserId,
    int BookId,
    DateTime ReturnDate);