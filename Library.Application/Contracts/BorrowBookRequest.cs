using System.ComponentModel.DataAnnotations;

namespace Library.Application.Contracts;

public record BorrowBookRequest(
    [Required] string UserId,
    [Required] int BookId,
    [Required] DateTime ReturnDate
    );