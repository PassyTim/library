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

public class CreateAuthorUseCaseTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly CreateAuthorUseCase _createAuthorUseCase;
    private readonly Mock<IAuthorsRepository> _authorsRepositoryMock;

    private readonly AuthorRequest _baseAuthorRequest = new(
        Id: 0,
        FirstName: "Test",
        LastName: "Test",
        Country: "Test",
        BirthDate: DateTime.Today);

    public CreateAuthorUseCaseTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        
        _authorsRepositoryMock = new Mock<IAuthorsRepository>();
        
        _unitOfWorkMock.Setup(uow => uow.AuthorsRepository).Returns(_authorsRepositoryMock.Object);
        
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<MappingConfig>();
        });
        
        var mapper = config.CreateMapper();
        _createAuthorUseCase = new CreateAuthorUseCase(_unitOfWorkMock.Object, mapper);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldThrowValidationException_WhenAuthorWithNonZeroId()
    {
        // Arrange
        var authorRequest = _baseAuthorRequest with { Id = 1 };

        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(() => _createAuthorUseCase.ExecuteAsync(authorRequest));
    }

    [Fact]
    public async Task ExecuteAsync_ShouldCreateAuthor_WhenValidAuthor()
    {
        // Arrange
        var authorRequest = _baseAuthorRequest;
        var author = new Author 
            { 
                Id = 0, 
                FirstName = "Test", 
                LastName = "Test", 
                Country = "Test", 
                BirthDate = DateTime.Today,
                Books = []
            };
        
        _authorsRepositoryMock.Setup(repo => repo.CreateAsync(It.IsAny<Author>())).Returns(Task.CompletedTask);

        
        // Act
        await _createAuthorUseCase.ExecuteAsync(authorRequest);

        // Assert
        _authorsRepositoryMock.Verify(repo => repo.CreateAsync(It.Is<Author>(a => a.FirstName == authorRequest.FirstName)), Times.Once);
        _unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(), Times.Once);
    }
}
