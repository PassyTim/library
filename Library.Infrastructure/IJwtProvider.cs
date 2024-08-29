using Library.Domain.Models;

namespace Library.Infrastructure;

public interface IJwtProvider
{
    string Generate(User user, IList<string> roles);
}