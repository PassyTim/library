using Library.Domain.Models;
using Library.Persistence;
using Library.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Library.Tests.Persistence.Tests;

public class SqlBorrowedBooksShould
{
    private readonly ApplicationDbContext _context;
    private readonly BorrowedBook _borrowedBook = new BorrowedBook
    {
        Id = 1,
        BookId = 1,
        UserId = "user1",
        TakeDate = DateTime.Today,
        ReturnDate = DateTime.Today.AddDays(7)
    };
    
    private readonly BorrowedBook _borrowedBook2 = new BorrowedBook
    {
        Id = 2,
        BookId = 2,
        UserId = "user2",
        TakeDate = DateTime.Today,
        ReturnDate = DateTime.Today.AddDays(7)
    };

    public SqlBorrowedBooksShould()
    {
        DbContextOptionsBuilder dbOptions = new DbContextOptionsBuilder()
            .UseInMemoryDatabase(
                Guid.NewGuid().ToString()
            );
        _context = new ApplicationDbContext(dbOptions.Options);
    }
    
    [Fact]
    public async Task CreateBorrowedBook()
    {
        // Arrange
        var repository = new BorrowedBookRepository(_context);

        // Act
        await repository.CreateAsync(_borrowedBook);
        await _context.SaveChangesAsync();

        // Assert
        var createdBorrowedBook = await _context.BorrowedBooks.FirstOrDefaultAsync(b => b.Id == _borrowedBook.Id);
        Assert.NotNull(createdBorrowedBook);
        Assert.Equal("user1", createdBorrowedBook.UserId);
    }

    [Fact]
    public async Task GetBorrowedBookById()
    {
        // Arrange
        var repository = new BorrowedBookRepository(_context);
        
        await repository.CreateAsync(_borrowedBook);
        await _context.SaveChangesAsync();

        // Act
        var retrievedBorrowedBook = await repository.GetByIdAsync(_borrowedBook.Id);

        // Assert
        Assert.NotNull(retrievedBorrowedBook);
        Assert.Equal(_borrowedBook.Id, retrievedBorrowedBook.Id);
    }

    [Fact]
    public async Task GetAllBorrowedBooksByUserId()
    {
        // Arrange
        var repository = new BorrowedBookRepository(_context);
        var borrowedBook = new BorrowedBook
        {
            Id = 3,
            BookId = 3,
            UserId = "user3",
            TakeDate = DateTime.Today,
            ReturnDate = DateTime.Today.AddDays(7)
        };
        var borrowedBook2 = new BorrowedBook
        {
            Id = 4,
            BookId = 4,
            UserId = "user3",
            TakeDate = DateTime.Today,
            ReturnDate = DateTime.Today.AddDays(10)
        };

        await repository.CreateAsync(borrowedBook);
        await repository.CreateAsync(borrowedBook2);
        await _context.SaveChangesAsync();

        // Act
        var borrowedBooks = await repository.GetAllByUserId("user3");

        // Assert
        Assert.NotNull(borrowedBooks);
        Assert.Equal(2, borrowedBooks.Count);
    }

    [Fact]
    public async Task CheckIfBorrowedBookExists()
    {
        // Arrange
        var repository = new BorrowedBookRepository(_context);
        
        await repository.CreateAsync(_borrowedBook);
        await _context.SaveChangesAsync();

        // Act
        var exists = await repository.IsBorrowedBookExistAsync(_borrowedBook.Id);

        // Assert
        Assert.True(exists);
    }

    [Fact]
    public async Task CheckIfBorrowedBookDoesNotExist()
    {
        // Arrange
        var repository = new BorrowedBookRepository(_context);
        int nonExistentBorrowedBookId = 999;

        // Act
        var exists = await repository.IsBorrowedBookExistAsync(nonExistentBorrowedBookId);

        // Assert
        Assert.False(exists);
    }
}