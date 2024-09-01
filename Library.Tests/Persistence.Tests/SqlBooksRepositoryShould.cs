using Library.Domain.Models;
using Library.Persistence;
using Library.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Library.Tests.Persistence.Tests;

public class SqlBooksRepositoryShould
{
    private readonly ApplicationDbContext _context;
    private readonly Book _book = new()
    {
        Id = 0,
        AuthorId = 1,
        Name = "Test",
        Genre = "test",
        Isbn = "9781234567897",
        Description = "test",
        AvailableCount = 1,
        TotalCount = 1,
        ImagePath = "test"
    };
    private readonly Book _book2 = new()
    {
        Id = 0,
        AuthorId = 2,
        Name = "Test 2",
        Genre = "test",
        Isbn = "9781234567898",
        Description = "test",
        AvailableCount = 2,
        TotalCount = 2,
        ImagePath = "test"
    };

    public SqlBooksRepositoryShould()
    {
        DbContextOptionsBuilder dbOptions = new DbContextOptionsBuilder()
            .UseInMemoryDatabase(
                Guid.NewGuid().ToString()
            );
        _context = new ApplicationDbContext(dbOptions.Options);
    }

    [Fact]
    public async Task CreateBook()
    {
        // Arrange
        var repository = new BooksRepository(_context);
        Book book = _book;
        
        // Act
        await repository.CreateAsync(book);
        await _context.SaveChangesAsync();
        
        // Assert
        Book returnBook = _context.Books.FirstOrDefault(); 
        Assert.NotNull(returnBook);
        Assert.Equal(book.Name, returnBook.Name);
        Assert.Equal(book.Isbn, returnBook.Isbn);
    }
    [Fact]
    public async Task GetAllBooks()
    {
        // Arrange
        var repository = new BooksRepository(_context);
        await repository.CreateAsync(_book);
        await repository.CreateAsync(_book2);
        await _context.SaveChangesAsync();

        // Act
        var allBooks = await repository.GetAllAsync();

        // Assert
        Assert.NotNull(allBooks);
        Assert.Equal(2, allBooks.Count);
        Assert.Contains(allBooks, b => b.Name == _book.Name);
        Assert.Contains(allBooks, b => b.Name == _book2.Name);
    }

    [Fact]
    public async Task GetAllBooks_WithFilter()
    {
        // Arrange
        var repository = new BooksRepository(_context);
        await repository.CreateAsync(_book);
        await repository.CreateAsync(_book2);
        await _context.SaveChangesAsync();

        // Act
        var filteredBooks = await repository.GetAllAsync(b => b.AuthorId == 1);

        // Assert
        Assert.NotNull(filteredBooks);
        Assert.Single(filteredBooks);
        Assert.Equal(_book.Name, filteredBooks[0].Name);
    }

    [Fact]
    public async Task ReturnBookById()
    {
        // Arrange
        var repository = new BooksRepository(_context);
        Book book = _book;
        await repository.CreateAsync(book);
        await _context.SaveChangesAsync();

        // Act
        Book? returnBook = await repository.GetByIsbn(book.Isbn);

        // Assert
        Assert.NotNull(returnBook);
        Assert.Equal(book.Isbn, returnBook.Isbn);
    }
    
    [Fact]
    public async Task ReturnBookByIsbn()
    {
        // Arrange
        var repository = new BooksRepository(_context);
        Book book = _book;
        await repository.CreateAsync(book);
        await _context.SaveChangesAsync();

        // Act
        Book? returnBook = await repository.GetByIsbn(book.Isbn);

        // Assert
        Assert.NotNull(returnBook);
        Assert.Equal(book.Isbn, returnBook.Isbn);
    }
    
    [Fact]
    public async Task CheckIfBookWithIdExists()
    {
        // Arrange
        var repository = new BooksRepository(_context);
        Book book = _book;
        await repository.CreateAsync(book);
        await _context.SaveChangesAsync();

        // Act
        bool exists = await repository.IsBookWithIdExists(book.Id);

        // Assert
        Assert.True(exists);
    }

    [Fact]
    public async Task CheckIfIsbnIsUnique()
    {
        // Arrange
        var repository = new BooksRepository(_context);
        Book book = _book;
        await repository.CreateAsync(book);
        await _context.SaveChangesAsync();

        // Act
        bool isUnique = await repository.IsIsbnUnique("9781234567890");

        // Assert
        Assert.True(isUnique);
    }

    [Fact]
    public async Task CheckIfIsbnIsUniqueForUpdate()
    {
        // Arrange
        var repository = new BooksRepository(_context);
        Book book = _book;
        await repository.CreateAsync(book);
        await _context.SaveChangesAsync();

        // Act
        bool isUnique = await repository.IsIsbnUniqueForUpdate("9781234567897", book.Id);

        // Assert
        Assert.True(isUnique);
    }
}