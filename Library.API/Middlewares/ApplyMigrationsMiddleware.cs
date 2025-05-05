using Library.Persistence;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Library.API.Middlewares;

public static class ApplyMigrationsMiddleware
{
    public async static Task ApplyMigrations(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();

        await using var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        var migrator = dbContext.GetService<IMigrator>();
        await migrator.MigrateAsync();
    }
}