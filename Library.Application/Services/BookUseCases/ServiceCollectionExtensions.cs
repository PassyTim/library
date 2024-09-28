using Microsoft.Extensions.DependencyInjection;

namespace Library.Application.Services.BookUseCases;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddBookUseCases(this IServiceCollection services)
    {
        services.AddScoped<CreateBookUseCase>();
        services.AddScoped<GetAllBooksUseCase>();
        services.AddScoped<GetBookByIdUseCase>();
        services.AddScoped<GetBookByIsbnUseCase>();
        services.AddScoped<GetUserTakenBooksUseCase>();
        services.AddScoped<RemoveBookUseCase>();
        services.AddScoped<ReturnBookUseCase>();
        services.AddScoped<TakeBookUseCase>();
        services.AddScoped<UpdateBookUseCase>();
        services.AddScoped<GetFilteredBooksUseCase>();

        return services;
    }
}