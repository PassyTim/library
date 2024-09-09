namespace Library.Domain.Models;

public class Book
{
    public int Id { get; set; }
    public string Isbn { get; set; }
    public string Name { get; set; }
    public string Genre { get; set; }
    public string Description { get; set; }
    public string ImagePath { get; set; }
    public int AuthorId { get; set; }
    public Author Author { get; set; }
    public User? User { get; set; }
    public string? UserId { get; set; }
    public DateTime TakeDate { get; set; }
    public DateTime ReturnDate { get; set; }
}