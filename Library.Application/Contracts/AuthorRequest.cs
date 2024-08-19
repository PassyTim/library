using System.ComponentModel.DataAnnotations;
using Library.Domain;

namespace Library.Application.Contracts;

public record AuthorRequest(
    [Required] int Id,
    [Required][MaxLength(Constants.AuthorFirstNameMaxLength)] string FirstName,
    [Required][MaxLength(Constants.AuthorLastNameMaxLength)] string LastName,
    [MaxLength(Constants.AuthorCountryMaxLength)] string Country,
    DateTime BirthDate
    );