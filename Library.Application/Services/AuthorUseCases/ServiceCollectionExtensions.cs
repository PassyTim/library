using Library.Application.Services.AuthorUseCases;
using Microsoft.Extensions.DependencyInjection;

namespace Library.Application.Services.AuthorService;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAuthorUseCases(this IServiceCollection services)
    {
        services.AddScoped<CreateAuthorUseCase>();
        services.AddScoped<GetAllAuthorsUseCase>();
        services.AddScoped<GetAuthorByIdUseCase>();
        services.AddScoped<RemoveAuthorUseCase>();
        services.AddScoped<UpdateAuthorUseCase>();
        

        return services;
    }
}