using AutoMapper;
using FluentValidation;
using Library.Application;
using Library.Application.Contracts.AuthorContracts;
using Library.Application.Services.AuthorUseCases;
using Library.Domain.IRepositories;
using Library.Domain.Models;
using Library.Persistence.UnitOfWork;
using Moq;

namespace Library.Tests.Application.Tests;

public class UpdateAuthorUseCaseTests
{
    private readonly UpdateAuthorUseCase _updateAuthorUseCase;
    private readonly Mock<IAuthorsRepository> _authorsRepositoryMock;
    
    private readonly AuthorRequest _baseAuthorRequest = new(
        Id: 0,
        FirstName: "Test",
        LastName: "Test",
        Country: "Test",
        BirthDate: DateTime.Today);

    public UpdateAuthorUseCaseTests()
    {
        Mock<IUnitOfWork> unitOfWorkMock = new();
        _authorsRepositoryMock = new Mock<IAuthorsRepository>();
        unitOfWorkMock.Setup(uow => uow.AuthorsRepository).Returns(_authorsRepositoryMock.Object);
        
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<MappingConfig>();
        });
        
        var mapper = config.CreateMapper();
        _updateAuthorUseCase = new UpdateAuthorUseCase(unitOfWorkMock.Object, mapper);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldThrowValidationException_WhenAuthorToUpdateIsNotFound()
    {
        //Arrange
        var authorId = 1;
        _authorsRepositoryMock.Setup(repo => repo.GetById(1)).ReturnsAsync((Author?)null);
        var authorUpdateRequest = _baseAuthorRequest with { Id = 1 };
        
        //Act & Assert
        await Assert.ThrowsAsync<ValidationException>(() => _updateAuthorUseCase.ExecuteAsync(authorUpdateRequest, authorId));
    }

    [Fact]
    public async Task ExecuteAsync_ShouldThrowValidationException_WhenIdsNotMatch()
    {
        //Arrange
        var authorId = 0;
        var author = new Author
        {
            Id = 0,
            FirstName = "Test",
            LastName = "Test",
            Country = "Test",
            BirthDate = DateTime.Today,
            Books = []
        };
        
        _authorsRepositoryMock.Setup(repo => repo.GetById(1)).ReturnsAsync(author);
        var authorUpdateRequest = _baseAuthorRequest with { Id = 1 };
        
        //Act & Assert
        await Assert.ThrowsAsync<ValidationException>(() => _updateAuthorUseCase.ExecuteAsync(authorUpdateRequest, authorId));
    }

    [Fact]
    public async Task ExecuteAsync_ShouldUpdateAuthorNameToTest2_WhenAuthorIsCorrect()
    {
        //Arrange
        var authorId = 1;
        var author = new Author
        {
            Id = 1,
            FirstName = "Test",
            LastName = "Test",
            Country = "Test",
            BirthDate = DateTime.Today,
            Books = []
        };
        
        _authorsRepositoryMock.Setup(repo => repo.GetById(authorId)).ReturnsAsync(author);
        var authorUpdateRequest = _baseAuthorRequest with { Id = 1, FirstName = "Test2"};
        
        //Act
        await _updateAuthorUseCase.ExecuteAsync(authorUpdateRequest, authorId);
        
        //Assert
        _authorsRepositoryMock.Verify(repo => repo.UpdateAsync(It.Is<Author>(a => a.FirstName == "Test2")), Times.Once);
    }
}