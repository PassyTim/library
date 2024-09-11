using Library.Domain.IRepositories;
using Library.Persistence.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Library.Persistence;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IBooksRepository, BooksRepository>();
        services.Decorate<IBooksRepository, CachedBooksRepository>();
        services.AddScoped<IAuthorsRepository, AuthorsRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        
        return services;
    }
}