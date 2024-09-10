using System.ComponentModel.DataAnnotations;
using Library.Domain;
using Library.Domain.Models;

namespace Library.Application.Contracts;

public record BookResponse(
    int Id,
    string Isbn,
    string Name,
    string Genre,
    string? Description,
    DateTime? ReturnDate,
    DateTime? TakeDate,
    string? UserId,
    int AuthorId)
{
    public string ImageUrl { get; set; }
};
