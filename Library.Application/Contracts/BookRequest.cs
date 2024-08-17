using System.ComponentModel.DataAnnotations;

namespace Library.Application.Contracts;

public record BookRequest(
    int Id,
    [Required] [MaxLength(20)] string Isbn,
    [Required] [MaxLength(100)] string Name,
    [Required] [MaxLength(20)] string Genre,
    string Description,
    string ImageUrl,
    int AuthorId);