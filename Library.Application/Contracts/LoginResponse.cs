using Library.Domain.Models;

namespace Library.Application.Contracts;

public class LoginResponse
{
    public ResponseUser User { get; set; }
    public string Token { get; set; }
}