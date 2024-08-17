using System.ComponentModel.DataAnnotations;

namespace Library.Application.Contracts;

public record AuthorRequest(
    [Required] int Id,
    [Required][MaxLength(50)] string FirstName,
    [Required][MaxLength(50)] string LastName,
    [MaxLength(60)] string Country,
    DateTime BirthDate
    );