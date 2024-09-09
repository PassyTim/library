using Library.Domain.Models;
using Library.Persistence;
using Library.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Library.Tests.Persistence.Tests;

public class SqlUsersRepositoryShould
{
    private readonly ApplicationDbContext _context;

    private readonly User _user = new()
    {
        Id = "1",
        UserName = "testuser",
        Email = "test@test.com",
        RefreshToken = "sampleRefreshToken"
    };
    
    public SqlUsersRepositoryShould()
    {
        DbContextOptionsBuilder dbOptions = new DbContextOptionsBuilder()
            .UseInMemoryDatabase(
                Guid.NewGuid().ToString()
            );
        _context = new ApplicationDbContext(dbOptions.Options);
    }
    
    [Fact]
    public async Task GetUserByEmail()
    {
        // Arrange
        var repository = new UsersRepository(_context);
        
        _context.Users.Add(_user);
        await _context.SaveChangesAsync();

        // Act
        var retrievedUser = await repository.GetByEmail(_user.Email);

        // Assert
        Assert.NotNull(retrievedUser);
        Assert.Equal(_user.Email, retrievedUser.Email);
    }

    [Fact]
    public async Task GetUserByRefreshToken()
    {
        // Arrange
        var repository = new UsersRepository(_context);
        
        _context.Users.Add(_user);
        await _context.SaveChangesAsync();

        // Act
        var retrievedUser = await repository.GetByRefreshToken(_user.RefreshToken);

        // Assert
        Assert.NotNull(retrievedUser);
        Assert.Equal(_user.RefreshToken, retrievedUser.RefreshToken);
    }

    [Fact]
    public async Task ReturnNullIfUserNotFoundByEmail()
    {
        // Arrange
        var repository = new UsersRepository(_context);

        // Act
        var retrievedUser = await repository.GetByEmail("nonexistent@example.com");

        // Assert
        Assert.Null(retrievedUser);
    }

    [Fact]
    public async Task ReturnNullIfUserNotFoundByRefreshToken()
    {
        // Arrange
        var repository = new UsersRepository(_context);

        // Act
        var retrievedUser = await repository.GetByRefreshToken("nonexistentRefreshToken");

        // Assert
        Assert.Null(retrievedUser);
    }
}