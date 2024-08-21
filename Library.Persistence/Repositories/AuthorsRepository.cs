using System.Linq.Expressions;
using Library.Domain.IRepositories;
using Library.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Library.Persistence.Repositories;

public class AuthorsRepository(ApplicationDbContext dbContext) : IAuthorsRepository
{
    public async Task<List<Author>> GetAllAsync(Expression<Func<Author, bool>>? filter = null,
        int pageSize = 0, int pageNumber = 0)
    {
        IQueryable <Author> query = dbContext.Authors;
        
        if (filter is not null) query = query.Where(filter);
        
        if (pageSize > 0)
        {
            if (pageSize > 100) pageSize = 100;

            query = query.Skip(pageSize * (pageNumber - 1)).Take(pageSize);
        }
        
        return await query
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Author?> GetById(int id)
    {
        return await dbContext.Authors
            .AsNoTracking()
            .FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task<Author?> GetByIdWithBooks(int id)
    {
        return await dbContext.Authors
            .AsNoTracking()
            .Include(a => a.Books)
            .FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task CreateAsync(Author author)
    {
        await dbContext.Authors
            .AddAsync(author);
    }

    public async Task UpdateAsync(Author author)
    {
        await dbContext.Authors
            .Where(a => a.Id == author.Id)
            .ExecuteUpdateAsync(s =>
                s.SetProperty(a => a.Country, author.Country)
                    .SetProperty(a => a.FirstName, author.FirstName)
                    .SetProperty(a => a.LastName, author.LastName)
                    .SetProperty(a => a.BirthDate, author.BirthDate));
    }
    
    public async Task<bool> IsAuthorWithIdExists(int id)
    {
        return await dbContext.Authors.AnyAsync(a => a.Id == id);
    }

    public async Task RemoveAsync(int authorId)
    {
        await dbContext.Authors
            .Where(a=>a.Id == authorId)
            .ExecuteDeleteAsync();
    }
}