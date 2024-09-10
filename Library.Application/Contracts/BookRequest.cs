using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Library.Application.Contracts;

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