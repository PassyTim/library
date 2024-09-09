using System.Linq.Expressions;
using AutoMapper;
using FluentValidation;
using Library.Application.Contracts;
using Library.Application.Exceptions;
using Library.Application.IServices;
using Library.Domain.Models;
using Library.Persistence;

namespace Library.Application.Services;

public class AuthorService(
    IUnitOfWork unitOfWork,
    IMapper mapper,
    IValidator<AuthorRequest> validator) : IAuthorService
{
    public async Task<List<AuthorResponse>> GetAll(Expression<Func<Author, bool>>? filter = null,
        int pageSize = 0, int pageNumber = 0)
    {
        if (pageSize > 100) pageSize = 100;
        
        var authors = await unitOfWork.AuthorsRepository.GetAllAsync(filter, pageSize, pageNumber);
        var authorResponseList = mapper.Map<List<AuthorResponse>>(authors);
        return authorResponseList;
    }

    public async Task<AuthorResponse> GetById(int id)
    {
        var author = await unitOfWork.AuthorsRepository.GetById(id);
        
        if (author is null)
        {
            throw new ItemNotFoundException($"Author with id:{id} not found");
        }
        
        var authorResponse = mapper.Map<AuthorResponse>(author);
        return authorResponse;
    }

    public async Task Create(AuthorRequest authorCreateRequest)
    {
        var validationContext = new ValidationContext<AuthorRequest>(authorCreateRequest);
        validationContext.RootContextData["IsCreate"] = true;
        var validationResult = await validator.ValidateAsync(validationContext);
        
        if (!validationResult.IsValid)
        {
            var errorMessages = validationResult.Errors.Select(x => x.ErrorMessage);
            throw new ValidationException(string.Join(". ", errorMessages));
        }
        
        var authorToCreate = mapper.Map<Author>(authorCreateRequest);
        await unitOfWork.AuthorsRepository.CreateAsync(authorToCreate);
        await unitOfWork.SaveChangesAsync();
    }

    public async Task Update(AuthorRequest authorUpdateRequest, int id)
    {
        var validationContext = new ValidationContext<AuthorRequest>(authorUpdateRequest);
        validationContext.RootContextData["IsUpdate"] = true;
        validationContext.RootContextData["Id"] = id;
        var validationResult = await validator.ValidateAsync(validationContext);

        if (!validationResult.IsValid)
        {
            var errorMessages = validationResult.Errors.Select(x => x.ErrorMessage);
            throw new ValidationException(string.Join(". ", errorMessages));
        }
        
        var authorToUpdate = mapper.Map<Author>(authorUpdateRequest);
        await unitOfWork.AuthorsRepository.UpdateAsync(authorToUpdate);
        await unitOfWork.SaveChangesAsync();
    }

    public async Task Remove(int id)
    {
        var author = await unitOfWork.AuthorsRepository.GetById(id);
        if (author is null)
        {
            throw new ItemNotFoundException($"Author with id:{id} not found");
        }
        
        await unitOfWork.AuthorsRepository.RemoveAsync(id);
    }
}