using Library.Application.Contracts.BookContracts;

namespace Library.Application.Contracts.AuthorContracts;

public record AuthorResponse(
    int Id,
    string FirstName,
    string LastName,
    string Country,
    DateTime BirthDate,
    List<BookResponse> Books);