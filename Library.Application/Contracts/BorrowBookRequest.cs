using System.ComponentModel.DataAnnotations;

namespace Library.Application.Contracts;

public record BorrowBookRequest(
    string UserId,
    int BookId,
    DateTime ReturnDate
    );