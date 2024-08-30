using Microsoft.AspNetCore.Identity;

namespace Library.Application.Contracts;

public class RegisterResponse
{
    public bool IsSuccess { get; set; }
    public IList<string> Errors { get; set; } = [];
}