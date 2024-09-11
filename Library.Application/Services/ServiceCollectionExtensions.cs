using Library.Application.IServices;
using Microsoft.Extensions.DependencyInjection;

namespace Library.Application.Services;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddUseCaseServices(this IServiceCollection services)
    {
        services.AddScoped<IBookService, BookService.BookService>();
        services.AddScoped<IAuthorService, AuthorService.AuthorService>();

        return services;
    }
}