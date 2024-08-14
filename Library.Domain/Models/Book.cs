namespace Library.Domain.Models;

public class Book
{
    public int Id { get; private set; }
    public string Isbn { get; private set; }
    public string Name { get; private set; }
    public string Genre { get; private set; }
    public string Description { get; private set; }
    public string AuthorId { get; private set; }
    public Author Author { get; private set; }
    public string ImageUrl { get; private set; }
    public DateTime TakeDate { get; private set; }
    public DateTime ReturnDate { get; private set; }
}