using Library.Domain.IRepositories;
using Library.Persistence.Repositories;
using Microsoft.Extensions.Caching.Memory;

namespace Library.Persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IMemoryCache _memoryCache;
    private IBooksRepository? _booksRepository;
    private IAuthorsRepository? _authorsRepository;
    private IUsersRepository? _usersRepository;
    private bool _isDisposed;

    public UnitOfWork(ApplicationDbContext dbContext, IMemoryCache memoryCache)
    {
        _dbContext = dbContext;
        _memoryCache = memoryCache;
    }

    public IBooksRepository BooksRepository
    {
        get
        {
            if (_booksRepository is null)
            {
                _booksRepository = new CachedBooksRepository(new BooksRepository(_dbContext), _memoryCache);
            }

            return _booksRepository;
        }
    }
    
    public IUsersRepository UsersRepository
    {
        get
        {
            if (_usersRepository is null)
            {
                _usersRepository = new UsersRepository(_dbContext);
            }

            return _usersRepository;
        }
    }

    public IAuthorsRepository AuthorsRepository
    {
        get
        {
            if (_authorsRepository is null)
            {
                _authorsRepository = new AuthorsRepository(_dbContext);
            }

            return _authorsRepository;
        }
    }
    
    public async Task SaveChangesAsync()
    {
        await _dbContext.SaveChangesAsync();
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_isDisposed)
        {
            if (disposing)
            {
                _dbContext.Dispose();
            }
        }
        _isDisposed = true;
    }
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}