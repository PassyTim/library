using System.ComponentModel.DataAnnotations;
using Library.Domain;

namespace Library.Application.Contracts;

public record BookRequest(
    int Id,
    [Required] string Isbn,
    [Required] string Name,
    [Required] string Genre,
    string Description,
    string ImageUrl,
    int AuthorId);