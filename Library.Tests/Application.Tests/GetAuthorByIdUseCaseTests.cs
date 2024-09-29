using AutoMapper;
using Library.Application;
using Library.Application.Contracts.AuthorContracts;
using Library.Application.Contracts.BookContracts;
using Library.Application.Exceptions;
using Library.Application.Services.AuthorUseCases;
using Library.Domain.IRepositories;
using Library.Domain.Models;
using Library.Persistence.UnitOfWork;
using Moq;

namespace Library.Tests.Application.Tests;

public class GetAuthorByIdUseCaseTests
{
    private readonly GetAuthorByIdUseCase _getAuthorByIdUseCase;
    private readonly Mock<IAuthorsRepository> _authorsRepositoryMock;

    public GetAuthorByIdUseCaseTests()
    {
        Mock<IUnitOfWork> unitOfWorkMock = new();
        _authorsRepositoryMock = new Mock<IAuthorsRepository>();
        unitOfWorkMock.Setup(uow => uow.AuthorsRepository).Returns(_authorsRepositoryMock.Object);
        
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<MappingConfig>();
        });
        
        var mapper = config.CreateMapper();
        _getAuthorByIdUseCase = new GetAuthorByIdUseCase(unitOfWorkMock.Object, mapper);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldThrowItemNotFoundException_WhenAuthorIsNotFound()
    {
        //Arrange
        var authorId = 1;
        _authorsRepositoryMock.Setup(repo => repo.GetById(authorId)).ReturnsAsync((Author?)null);
        
        //Act &Assert
        await Assert.ThrowsAsync<ItemNotFoundException>(() => _getAuthorByIdUseCase.ExecuteAsync(authorId));
    }
    
    [Fact]
    public async Task ExecuteAsync_ShouldReturnAuthor_WhenAuthorIsFound()
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

        var expectedResponse = new AuthorResponse
        (
            Id: authorId,
            FirstName: "Test",
            LastName: "Test",
            Country: "Test",
            BirthDate: DateTime.Today,
            Books: []
        );

        _authorsRepositoryMock.Setup(repo => repo.GetById(authorId)).ReturnsAsync(author);

        //Act
        var result = await _getAuthorByIdUseCase.ExecuteAsync(authorId);
        
        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedResponse.Id, result.Id);
        Assert.Equal(expectedResponse.FirstName, result.FirstName);
        Assert.Equal(expectedResponse.LastName, result.LastName);
        Assert.Equal(expectedResponse.Country, result.Country);
        Assert.Equal(expectedResponse.BirthDate, result.BirthDate);
        Assert.Equal(expectedResponse.Books, result.Books);
    }
}