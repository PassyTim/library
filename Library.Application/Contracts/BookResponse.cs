using System.ComponentModel.DataAnnotations;
using Library.Domain;
using Library.Domain.Models;

namespace Library.Application.Contracts;

public record BookResponse(
    [Required] int Id,
    [Required] [MaxLength(Constants.BookIsbnMaxLength)]
    string Isbn,
    [Required] [MaxLength(Constants.BookNameMaxLength)]
    string Name,
    [Required] [MaxLength(Constants.BookGenreMaxLength)]
    string Genre,
    string Description,
    int TotalCount,
    int AvailableCount,
    int AuthorId)
{
    public string ImageUrl { get; set; }
};
