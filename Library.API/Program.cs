using FluentValidation;
using Library.API.Extensions;
using Library.API.Middlewares;
using Library.Application;
using Library.Application.Services;
using Library.Application.Services.AuthorService;
using Library.Application.Services.BookService;
using Library.Application.Services.UserUseCases;
using Library.Application.Services.Validation;
using Library.Domain.Models;
using Library.Infrastructure.JwtProvider;
using Library.Persistence;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultSQLConnection"));
});

services.Configure<JwtOptions>(builder.Configuration.GetSection(nameof(JwtOptions)));
services.AddScoped<IJwtProvider, JwtProvider>();

services.AddTransient<GlobalExceptionHandlingMiddleware>();

services.AddRepositories();
services.AddMemoryCache();

services.AddUserUseCases();
services.AddBookUseCases();
services.AddAuthorUseCases();

services.AddUseCaseServices();

services.AddValidatorsFromAssemblyContaining(typeof(BooksValidator));
services.AddAutoMapper(typeof(MappingConfig));

services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 5 * 1024 * 1024;
});

services.AddHttpContextAccessor();

services.AddControllers().AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

services.AddEndpointsApiExplorer();
services.AddSwagger();

services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

services.AddApiAuthentication(builder.Configuration);

services.ConfigureApplicationCookie(options =>
{
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(10);
});

services.AddConfiguredCors();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.ApplyMigrations();
}

app.UseCors();

app.UseHttpsRedirection();
app.UseStaticFiles(new StaticFileOptions
{
    OnPrepareResponse = ctx =>
    {
        const int durationInSeconds = 60 * 60 * 24;
        ctx.Context.Response.Headers[HeaderNames.CacheControl] = "public,max-age=" + durationInSeconds;
        ctx.Context.Response.Headers[HeaderNames.Expires] = new[] { DateTime.UtcNow.AddYears(1).ToString("R") };
    }
});

app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
