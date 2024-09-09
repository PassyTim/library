using Library.Domain.Models;
using Library.Persistence;
using Library.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Library.Tests.Persistence.Tests;

public class SqlAuthorsRepositoryShould
{
    private readonly ApplicationDbContext _context;

    private readonly Author _author1 = new()
    {
        Id = 0,
        FirstName = "Test",
        LastName = "Test",
        BirthDate = DateTime.Today,
        Country = "Test"
    };
    private readonly Author _author2 = new()
    {
        Id = 0,
        FirstName = "Test 2",
        LastName = "Test 2",
        BirthDate = DateTime.Today,
        Country = "Test 2"
    };

    public SqlAuthorsRepositoryShould()
    {
        DbContextOptionsBuilder dbOptions = new DbContextOptionsBuilder()
            .UseInMemoryDatabase(
                Guid.NewGuid().ToString()
            );
        _context = new ApplicationDbContext(dbOptions.Options);
    }

    [Fact]
    public async Task CreateAuthor()
    {
        // Arrange
        var repository = new AuthorsRepository(_context);
            
        // Act
        await repository.CreateAsync(_author1);
        await _context.SaveChangesAsync();

        // Assert
        var returnAuthor = await _context.Authors.FirstOrDefaultAsync(a => a.Id == _author1.Id);
        Assert.NotNull(returnAuthor);
        Assert.Equal(_author1.FirstName, returnAuthor.FirstName);
        Assert.Equal(_author1.LastName, returnAuthor.LastName);
    }
    
    [Fact]
    public async Task ReturnAuthorById()
    {
        // Arrange
        var repository = new AuthorsRepository(_context);
        await repository.CreateAsync(_author1);
        await _context.SaveChangesAsync();

        // Act
        var returnAuthor = await repository.GetById(_author1.Id);

        // Assert
        Assert.NotNull(returnAuthor);
        Assert.Equal(_author1.Id, returnAuthor.Id);
    }

    [Fact]
    public async Task ReturnAuthorByIdWithBooks()
    {
        // Arrange
        var repository = new AuthorsRepository(_context);
        var book = new Book 
            { 
                Id = 0,
                AuthorId = _author1.Id, 
                Name = "Test Book",
                Genre = "test",
                Isbn = "9781234567898",
                Description = "test",
                ImagePath = "test"
            };
        
        await repository.CreateAsync(_author1);
        await _context.Books.AddAsync(book);
        await _context.SaveChangesAsync();

        // Act
        var returnAuthor = await repository.GetById(_author1.Id);

        // Assert
        Assert.NotNull(returnAuthor);
        Assert.Equal(_author1.Id, returnAuthor.Id);
        Assert.NotNull(returnAuthor.Books);
    }

    [Fact]
    public async Task CheckIfAuthorWithIdExists()
    {
        // Arrange
        var repository = new AuthorsRepository(_context);
        await repository.CreateAsync(_author1);
        await _context.SaveChangesAsync();

        // Act
        bool exists = await repository.IsAuthorWithIdExists(_author1.Id);

        // Assert
        Assert.True(exists);
    }

    [Fact]
    public async Task GetAllAuthors()
    {
        // Arrange
        var repository = new AuthorsRepository(_context);
        await repository.CreateAsync(_author1);
        await repository.CreateAsync(_author2);
        await _context.SaveChangesAsync();

        // Act
        var allAuthors = await repository.GetAllAsync();

        // Assert
        Assert.NotNull(allAuthors);
        Assert.Equal(2, allAuthors.Count);
        Assert.Contains(allAuthors, a => a.FirstName == _author1.FirstName);
        Assert.Contains(allAuthors, a => a.FirstName == _author2.FirstName);
    }

    [Fact]
    public async Task GetAllAuthors_WithFilter()
    {
        // Arrange
        var repository = new AuthorsRepository(_context);
        await repository.CreateAsync(_author1);
        await repository.CreateAsync(_author2);
        await _context.SaveChangesAsync();

        // Act
        var filteredAuthors = await repository.GetAllAsync(a => a.Country == "Test");

        // Assert
        Assert.NotNull(filteredAuthors);
        Assert.Single(filteredAuthors);
        Assert.Equal(_author1.FirstName, filteredAuthors[0].FirstName);
    }
}