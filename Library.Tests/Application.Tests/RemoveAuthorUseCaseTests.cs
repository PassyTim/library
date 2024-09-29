using Library.Application.Exceptions;
using Library.Application.Services.AuthorUseCases;
using Library.Domain.IRepositories;
using Library.Domain.Models;
using Library.Persistence.UnitOfWork;
using Moq;

namespace Library.Tests.Application.Tests;

public class RemoveAuthorUseCaseTests 
{
    private readonly RemoveAuthorUseCase _removeAuthorUseCase;
    private readonly Mock<IAuthorsRepository> _authorsRepositoryMock;

    public RemoveAuthorUseCaseTests()
    {
        Mock<IUnitOfWork> unitOfWorkMock = new();
        _authorsRepositoryMock = new Mock<IAuthorsRepository>();
        
        unitOfWorkMock.Setup(uow => uow.AuthorsRepository).Returns(_authorsRepositoryMock.Object);
        
        _removeAuthorUseCase = new RemoveAuthorUseCase(unitOfWorkMock.Object);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldThrowItemNotFoundException_WhenAuthorIsNotFound()
    {
        var authorId = 1;
        _authorsRepositoryMock.Setup(repo => repo.GetById(authorId)).ReturnsAsync((Author?)null);
        
        //Act &Assert
        await Assert.ThrowsAsync<ItemNotFoundException>(() => _removeAuthorUseCase.ExecuteAsync(authorId));
    }

    [Fact]
    public async Task ExecuteAsync_ShouldRemoveAuthor_WhenAuthorIsFound()
    {
        //Arrange
        var authorId = 1;
        var author = new Author
        {
            Id = authorId,
            FirstName = "Test",
            LastName = "Test",
            Country = "Test",
            BirthDate = DateTime.Today,
            Books = []
        };

        _authorsRepositoryMock.Setup(repo => repo.GetById(authorId)).ReturnsAsync(author);
        
        //Act
        await _removeAuthorUseCase.ExecuteAsync(authorId);
        
        //Assert
        _authorsRepositoryMock.Verify(repo => repo.RemoveAsync(authorId), Times.Once);
    }
}