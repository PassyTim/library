using Library.Domain;
using Library.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.Persistence.Configurations;

public class BookConfiguration : IEntityTypeConfiguration<Book>
{
    public void Configure(EntityTypeBuilder<Book> builder)
    {
        builder.HasKey(p => p.Id);
        builder.HasIndex(p => p.Isbn).IsUnique();

        builder.Property(p => p.AvailableCount).IsRequired();
        builder.Property(p => p.TotalCount).IsRequired();
        builder.Property(p => p.Name).IsRequired().HasMaxLength(Constants.BookNameMaxLength);
        builder.Property(p => p.Isbn).IsRequired().HasMaxLength(Constants.BookIsbnMaxLength);
        builder.Property(p => p.Genre).IsRequired().HasMaxLength(Constants.BookGenreMaxLength);

        builder.HasData(
            new Book
            {
                Id = 1,
                Isbn = "9785171183661",
                Name = "Евгений Онегин",
                Genre = "Новелла",
                Description = "Новелла, написанная Александром Пушкиным",
                ImagePath = "8fb16b40-17d4-43fa-b3fa-20238b342ad3.jpg",
                AuthorId = 1,
                TotalCount = 3,
                AvailableCount = 3
            },
            new Book
            {
                Id = 2,
                Isbn = "9781101948294",
                Name = "The Shining",
                Genre = "Хоррор",
                Description = "Хоррор, написанный Стивеном Кингом",
                ImagePath = "theShining.jpg",
                AuthorId = 2,
                TotalCount = 3,
                AvailableCount = 3
            },
            new Book
            {
                Id = 3,
                Isbn = "9785171183685",
                Name = "Преступление и наказание",
                Genre = "Философская новелла",
                Description = "Философская новелла, написанная Федором Достоевским.",
                ImagePath = "95a036bc205187af0456953a28ccccb1.jpeg",
                AuthorId = 3,
                TotalCount = 3,
                AvailableCount = 3
            },
            new Book
            {
                Id = 4,
                Isbn = "9785171183708",
                Name = "Братья Карамазовы",
                Genre = "Философская новелла",
                Description = "Философская новелла, написанная Федором Достоевским.",
                ImagePath = "a6d50e17-c422-4c07-b73d-3b9e722fa1bb.jpg",
                AuthorId = 3,
                TotalCount = 3,
                AvailableCount = 3
            },
            new Book
            {
                Id = 5,
                Isbn = "9780743273565",
                Name = "Оно",
                Genre = "Хоррор",
                Description = "Хоррор, написанный Стивеном Кингом",
                ImagePath = "i750566.jpg",
                AuthorId = 2,
                TotalCount = 3,
                AvailableCount = 3
            });
    }
}