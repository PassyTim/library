using System.ComponentModel.DataAnnotations;
using Library.Domain;
using Library.Domain.Models;

namespace Library.Application.Contracts;

public record BookResponse(
    int Id,
    string Isbn,
    string Name,
    string Genre,
    string Description,
    int AuthorId)
{
    public string ImageUrl { get; set; }
};
