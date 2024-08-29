using Microsoft.AspNetCore.Identity;

namespace Library.Domain.Models;

public class User : IdentityUser
{
    public List<BorrowedBook> BorrowedBooks { get; set; } = [];
}