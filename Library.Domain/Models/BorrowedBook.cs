namespace Library.Domain.Models;

public class BorrowedBook
{
    public int Id { get; set; }
    public int BookId { get; set; }
    public Book Book { get; set; }
    public string UserId { get; set; }
    public User User { get; set; }
    public DateTime TakeDate { get; set; }
    public DateTime ReturnDate { get; set; }
}