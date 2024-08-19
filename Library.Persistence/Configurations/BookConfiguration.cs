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
        
        builder.Property(p => p.Name).IsRequired().HasMaxLength(Constants.BookNameMaxLength);
        builder.Property(p => p.Isbn).IsRequired().HasMaxLength(Constants.BookIsbnMaxLength);
        builder.Property(p => p.Genre).IsRequired().HasMaxLength(Constants.BookGenreMaxLength);

        builder.HasData(
            new Book
            {
                Id = 1,
                Isbn = "9785171183661",
                Name = "Eugene Onegin",
                Genre = "Novel",
                Description = "A novel in verse by Alexander Pushkin.",
                ImageUrl = "https://example.com/eugene_onegin.jpg",
                AuthorId = 1
            },
            new Book
            {
                Id = 2,
                Isbn = "9781101948294",
                Name = "The Shining",
                Genre = "Horror",
                Description = "A horror novel by Stephen King.",
                ImageUrl = "https://example.com/the_shining.jpg",
                AuthorId = 2
            },
            new Book
            {
                Id = 3,
                Isbn = "9785171183685",
                Name = "Crime and Punishment",
                Genre = "Philosophical Novel",
                Description = "A philosophical novel by Fyodor Dostoevsky.",
                ImageUrl = "https://example.com/crime_and_punishment.jpg",
                AuthorId = 3
            },
            new Book
            {
                Id = 4,
                Isbn = "9785171183708",
                Name = "The Brothers Karamazov",
                Genre = "Philosophical Novel",
                Description = "A philosophical novel by Fyodor Dostoevsky.",
                ImageUrl = "https://example.com/brothers_karamazov.jpg",
                AuthorId = 3
            },
            new Book
            {
                Id = 5,
                Isbn = "9780743273565",
                Name = "It",
                Genre = "Horror",
                Description = "A horror novel by Stephen King.",
                ImageUrl = "https://example.com/it.jpg",
                AuthorId = 2
            });
    }
}