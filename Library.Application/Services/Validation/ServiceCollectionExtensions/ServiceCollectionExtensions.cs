using Microsoft.Extensions.DependencyInjection;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Enums;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;

namespace Library.Application.Services.Validation.ServiceCollectionExtensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAutoValidation(this IServiceCollection services)
    {
        services.AddFluentValidationAutoValidation(configuration =>
        {
            configuration.DisableBuiltInModelValidation = true;
            configuration.ValidationStrategy = ValidationStrategy.Annotations;
            configuration.EnableBodyBindingSourceAutomaticValidation = true;
            configuration.EnableFormBindingSourceAutomaticValidation = true;
        });

        return services;
    }
}