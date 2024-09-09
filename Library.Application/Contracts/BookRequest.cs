using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Library.Application.Contracts;

public record BookRequest(
    int Id,
    string Isbn,
    string Name,
    string Genre,
    string Description,
    int AuthorId, 
    int AvailableCount,
    int TotalCount,
    IFormFile? Image);