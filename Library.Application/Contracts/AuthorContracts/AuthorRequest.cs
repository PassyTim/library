namespace Library.Application.Contracts.AuthorContracts;

public record AuthorRequest(
    int Id,
    string FirstName,
    string LastName,
    string Country,
    DateTime BirthDate
    );