using Library.Domain.Models;

namespace Library.Application.Contracts;

public class ResponseUser
{
    public string Id { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public List<BorrowedBook> BorrowedBooks { get; set; }
}