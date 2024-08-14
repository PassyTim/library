namespace Library.Domain.Models;

public class Author
{
    public int Id { get; private set; }
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public DateTime BirthDate { get; private set; }
    public string Country { get; private set; }
}