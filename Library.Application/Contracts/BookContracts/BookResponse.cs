namespace Library.Application.Contracts.BookContracts;

public record BookResponse(
    int Id,
    string Isbn,
    string Name,
    string Genre,
    string? Description,
    DateTime? ReturnDate,
    DateTime? TakeDate,
    string? UserId,
    int AuthorId)
{
    public string ImageUrl { get; set; }
};
