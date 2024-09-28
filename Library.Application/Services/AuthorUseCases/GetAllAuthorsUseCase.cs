using AutoMapper;
using Library.Application.Contracts;
using Library.Application.Contracts.AuthorContracts;
using Library.Persistence.UnitOfWork;

namespace Library.Application.Services.AuthorUseCases;

public class GetAllAuthorsUseCase(
    IUnitOfWork unitOfWork,
    IMapper mapper)
{
    public async Task<List<AuthorResponse>> ExecuteAsync(int pageSize = 0, int pageNumber = 0)
    {
        if (pageSize > 100) pageSize = 100;
        
        var authors = await unitOfWork.AuthorsRepository.GetAllAsync(null, pageSize, pageNumber);
        var authorResponseList = mapper.Map<List<AuthorResponse>>(authors);
        return authorResponseList;
    }
}