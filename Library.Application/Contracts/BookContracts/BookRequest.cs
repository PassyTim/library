using Microsoft.AspNetCore.Http;

namespace Library.Application.Contracts.BookContracts;

public record BookRequest(
    int Id,
    string Isbn,
    string Name,
    string Genre,
    string? Description,
    int AuthorId,
    string? UserId,
    DateTime? ReturnDate,
    DateTime? TakeDate,
    IFormFile? Image);