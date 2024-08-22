using FluentValidation;
using Library.API.Middlewares;
using Library.Application;
using Library.Application.IServices;
using Library.Application.Services;
using Library.Domain.IRepositories;
using Library.Persistence;
using Library.Persistence.Repositories;
using Microsoft.AspNetCore.Http.Features;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

services.AddDbContext<ApplicationDbContext>();

services.AddTransient<GlobalExceptionHandlingMiddleware>();

services.AddScoped<IBooksRepository, BooksRepository>();
services.AddScoped<IAuthorsRepository, AuthorsRepository>();

services.AddScoped<IUnitOfWork, UnitOfWork>();

services.AddScoped<IBookService, BookService>();
services.AddScoped<IAuthorService, AuthorService>();

services.AddValidatorsFromAssemblyContaining(typeof(BooksValidator));
services.AddAutoMapper(typeof(MappingConfig));

services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 5 * 1024 * 1024;
});
services.AddHttpContextAccessor();

services.AddControllers();
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseMiddleware<GlobalExceptionHandlingMiddleware>();
app.MapControllers();

app.Run();
