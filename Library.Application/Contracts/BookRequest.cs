using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Library.Application.Contracts;

public record BookRequest(
    int Id,
    [Required] string Isbn,
    [Required] string Name,
    [Required] string Genre,
    string Description,
    int AuthorId, 
    [Required] int AvailableCount,
    [Required] int TotalCount,
    IFormFile? Image);