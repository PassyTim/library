using Library.Domain.Models;

namespace Library.Application.Contracts;

public record ResponseUser(
    string Id,
    string UserName,
    string Email);