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
        
        builder.Property(p => p.Name).IsRequired().HasMaxLength(100);
        builder.Property(p => p.Isbn).IsRequired().HasMaxLength(20);
        builder.Property(p => p.Genre).IsRequired().HasMaxLength(20);

        builder.HasData(
            new Book
            {
                Id = 1,
                Isbn = "978-5-17-118366-1",
                Name = "Eugene Onegin",
                Genre = "Novel",
                Description = "A novel in verse by Alexander Pushkin.",
                ImageUrl = "https://example.com/eugene_onegin.jpg",
                AuthorId = 1
            },
            new Book
            {
                Id = 2,
                Isbn = "978-1-101-94829-4",
                Name = "The Shining",
                Genre = "Horror",
                Description = "A horror novel by Stephen King.",
                ImageUrl = "https://example.com/the_shining.jpg",
                AuthorId = 2
            },
            new Book
            {
                Id = 3,
                Isbn = "978-5-17-118368-5",
                Name = "Crime and Punishment",
                Genre = "Philosophical Novel",
                Description = "A philosophical novel by Fyodor Dostoevsky.",
                ImageUrl = "https://example.com/crime_and_punishment.jpg",
                AuthorId = 3
            },
            new Book
            {
                Id = 4,
                Isbn = "978-5-17-118370-8",
                Name = "The Brothers Karamazov",
                Genre = "Philosophical Novel",
                Description = "A philosophical novel by Fyodor Dostoevsky.",
                ImageUrl = "https://example.com/brothers_karamazov.jpg",
                AuthorId = 3
            },
            new Book
            {
                Id = 5,
                Isbn = "978-0-7432-7356-5",
                Name = "It",
                Genre = "Horror",
                Description = "A horror novel by Stephen King.",
                ImageUrl = "https://example.com/it.jpg",
                AuthorId = 2
            });
    }
}