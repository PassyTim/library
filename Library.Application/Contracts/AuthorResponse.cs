using System.ComponentModel.DataAnnotations;
using Library.Domain;

namespace Library.Application.Contracts;

public record AuthorResponse(
    int Id,
    string FirstName,
    string LastName,
    string Country,
    DateTime BirthDate,
    List<BookResponse> Books);