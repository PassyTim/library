using Library.Domain.Models;

namespace Library.Domain.IRepositories;

public interface IUsersRepository
{
    public Task<User> GetByEmail(string email);
    public Task<User> GetByRefreshToken(string refreshToken);
}