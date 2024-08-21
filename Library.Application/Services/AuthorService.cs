using System.Linq.Expressions;
using AutoMapper;
using Library.Application.Contracts;
using Library.Application.IServices;
using Library.Domain.Models;
using Library.Persistence;

namespace Library.Application.Services;

public class AuthorService(
    IUnitOfWork unitOfWork,
    IMapper mapper) : IAuthorService
{
    public async Task<List<AuthorResponse>> GetAll(Expression<Func<Author, bool>>? filter = null,
        int pageSize = 0, int pageNumber = 0)
    {
        var authors = await unitOfWork.AuthorsRepository.GetAllAsync(filter, pageSize, pageNumber);
        var authorResponseList = mapper.Map<List<AuthorResponse>>(authors);
        return authorResponseList;
    }

    public async Task<AuthorResponse> GetById(int id, bool isWithBooks = false)
    {
        Author? author;
        AuthorResponse authorResponse;
        if (isWithBooks)
        {
            author = await unitOfWork.AuthorsRepository.GetByIdWithBooks(id);
            authorResponse = mapper.Map<AuthorResponse>(author);
            return authorResponse;
        }
        
        author = await unitOfWork.AuthorsRepository.GetById(id);
        authorResponse = mapper.Map<AuthorResponse>(author);
        return authorResponse;
    }

    public async Task Create(AuthorRequest authorCreateRequest)
    {
        var authorToCreate = mapper.Map<Author>(authorCreateRequest);
        await unitOfWork.AuthorsRepository.CreateAsync(authorToCreate);
        await unitOfWork.SaveChangesAsync();
    }

    public async Task Update(AuthorRequest authorUpdateRequest)
    {
        var authorToUpdate = mapper.Map<Author>(authorUpdateRequest);
        await unitOfWork.AuthorsRepository.UpdateAsync(authorToUpdate);
        await unitOfWork.SaveChangesAsync();
    }

    public async Task Remove(int id)
    {
        await unitOfWork.AuthorsRepository.RemoveAsync(id);
    }
}