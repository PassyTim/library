using Microsoft.AspNetCore.Identity;

namespace Library.Domain.Models;

public class User : IdentityUser
{
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiryTime { get; set; }
    public List<BorrowedBook> BorrowedBooks { get; set; } = [];
}