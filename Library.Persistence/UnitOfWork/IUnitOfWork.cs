using Library.Domain.IRepositories;

namespace Library.Persistence.UnitOfWork;

public interface IUnitOfWork : IDisposable
{
    IBooksRepository BooksRepository { get;}
    IAuthorsRepository AuthorsRepository { get; }
    IUsersRepository UsersRepository { get; }
    Task SaveChangesAsync();
}