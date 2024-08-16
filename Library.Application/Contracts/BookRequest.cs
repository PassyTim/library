using System.ComponentModel.DataAnnotations;

namespace Library.Application.Contracts;

public record BookRequest(
    [Required] string Isbn,
    [Required] string Name,
    [Required] string Genre,
    string Description,
    string ImageUrl,
    int AuthorId);