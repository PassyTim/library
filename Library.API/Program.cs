using FluentValidation;
using Library.API;
using Library.API.Extensions;
using Library.API.Middlewares;
using Library.Application;
using Library.Application.IServices;
using Library.Application.Services;
using Library.Application.Services.Validation;
using Library.Domain.IRepositories;
using Library.Domain.Models;
using Library.Infrastructure.JwtProvider;
using Library.Persistence;
using Library.Persistence.Repositories;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Net.Http.Headers;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultSQLConnection"));
});
services.Configure<JwtOptions>(builder.Configuration.GetSection(nameof(JwtOptions)));

services.AddTransient<GlobalExceptionHandlingMiddleware>();

services.AddScoped<IBooksRepository, BooksRepository>();
services.Decorate<IBooksRepository, CachedBooksRepository>();
services.AddMemoryCache();

services.AddScoped<IAuthorsRepository, AuthorsRepository>();

services.AddScoped<IUnitOfWork, UnitOfWork>();

services.AddScoped<IBookService, BookService>();
services.AddScoped<IAuthorService, AuthorService>();
services.AddScoped<UserService>();

services.AddScoped<IJwtProvider, JwtProvider>();

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
services.AddSwaggerGen();
services.AddSwagger();

services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

services.AddApiAuthentication(builder.Configuration);

services.ConfigureApplicationCookie(options =>
{
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(10);
});

services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://localhost:3000");
        policy.AllowAnyHeader();
        policy.AllowAnyMethod();
        policy.AllowCredentials();
        policy.WithExposedHeaders("x-count");
        policy.WithExposedHeaders("x-pagination");
        policy.WithExposedHeaders("Authorization");
        policy.WithExposedHeaders("Cache-Control");
    });
});

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
