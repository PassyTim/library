using Microsoft.Extensions.DependencyInjection;

namespace Library.Application.Services.UserUseCases.ServiceCollectionExtensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddUserUseCases(this IServiceCollection services)
    {
        services.AddScoped<LoginUserUseCase>();
        services.AddScoped<RegisterUserUseCase>();
        services.AddScoped<RefreshTokenUseCase>();

        return services;
    }
}