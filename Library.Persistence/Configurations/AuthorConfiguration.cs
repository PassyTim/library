using Library.Domain;
using Library.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.Persistence.Configurations;

public class AuthorConfiguration : IEntityTypeConfiguration<Author>
{
    public void Configure(EntityTypeBuilder<Author> builder)
    {
        builder.HasKey(p => p.Id);
        
        builder.Property(p => p.FirstName).IsRequired().HasMaxLength(Constants.AuthorFirstNameMaxLength);
        builder.Property(p => p.LastName).HasMaxLength(Constants.AuthorLastNameMaxLength);
        builder.Property(p => p.Country).HasMaxLength(Constants.AuthorCountryMaxLength);

        builder
            .HasMany(a => a.Books)
            .WithOne(b => b.Author)
            .HasForeignKey(b => b.AuthorId);

        builder.HasData(
            new Author
            {
                Id = 1,
                FirstName = "Александр",
                LastName = "Пушкин",
                Country = "Россия"
            },
            new Author
            {
                Id = 2,
                FirstName = "Стивен",
                LastName = "Кинг",
                Country = "США"
            },
            new Author
            {
                Id = 3,
                FirstName = "Федор",
                LastName = "Достоевский",
                Country = "Россия"
            },
            new Author
            {
                Id = 4,
                FirstName = "Масаси",
                LastName = "Кисимото",
                Country = "Япония"
            });
    }
}