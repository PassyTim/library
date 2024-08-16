using System.Reflection;
using FluentValidation;
using Library.Application;
using Library.Application.IServices;
using Library.Application.Services;
using Library.Domain.IRepositories;
using Library.Persistence;
using Library.Persistence.Repositories;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

services.AddDbContext<ApplicationDbContext>();

services.AddScoped<IBooksRepository, BooksRepository>();
services.AddScoped<IAuthorsRepository, AuthorsRepository>();

services.AddScoped<IBookService, BookService>();

services.AddValidatorsFromAssemblyContaining(typeof(BooksValidator));
services.AddAutoMapper(typeof(MappingConfig));

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
app.MapControllers();

app.Run();
