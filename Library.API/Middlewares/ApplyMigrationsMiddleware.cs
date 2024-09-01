using Library.Persistence;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Library.API.Middlewares;

public static class ApplyMigrationsMiddleware
{
    public static async void ApplyMigrations(this IApplicationBuilder app)
    {
        using IServiceScope scope = app.ApplicationServices.CreateScope();

        using ApplicationDbContext dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        var migrator = dbContext.GetService<IMigrator>();
        await migrator.MigrateAsync();
    }
}