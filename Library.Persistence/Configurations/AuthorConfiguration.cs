using Library.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.Persistence.Configurations;

public class AuthorConfiguration : IEntityTypeConfiguration<Author>
{
    public void Configure(EntityTypeBuilder<Author> builder)
    {
        builder.HasKey(p => p.Id);
        
        builder.Property(p => p.FirstName).IsRequired().HasMaxLength(50);
        builder.Property(p => p.LastName).HasMaxLength(50);
        builder.Property(p => p.Country).HasMaxLength(60);

        builder
            .HasMany(a => a.Books)
            .WithOne(b => b.Author)
            .HasForeignKey(b => b.AuthorId);

        builder.HasData(
            new Author
            {
                Id = 1,
                FirstName = "Aleksandr",
                LastName = "Pushkin",
                Country = "Russia"
            },
            new Author
            {
                Id = 2,
                FirstName = "Steven",
                LastName = "King",
                Country = "USA"
            },
            new Author
            {
                Id = 3,
                FirstName = "Fedor",
                LastName = "Dostoevskiy",
                Country = "Russia"
            });
    }
}