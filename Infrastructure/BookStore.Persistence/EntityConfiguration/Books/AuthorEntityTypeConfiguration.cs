using BookStore.Domain.Entities.Authors;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookStore.Persistence.EntityConfiguration.Books;
public class AuthorEntityTypeConfiguration : IEntityTypeConfiguration<Author>
{
    public void Configure(EntityTypeBuilder<Author> builder)
    {
        builder.ToTable("authors");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.FirstName)
               .IsRequired()
               .HasMaxLength(100);

        builder.Property(x => x.LastName)
               .IsRequired()
               .HasMaxLength(100);

        builder.Property(x => x.Description)
               .HasMaxLength(1000);

        builder.Property(x => x.BirthDay)
               .IsRequired();

        builder.Property(x => x.DeathTime)
              .HasDefaultValue(null);

        builder.Property(x => x.IsDeleted)
               .HasDefaultValue(false);

        builder.Property(b => b.DeletedAt)
            .HasDefaultValue(null);

        builder.Property(b => b.DeletedById)
            .HasDefaultValue(null);

        builder.Property(b => b.UpdatedAt)
            .HasDefaultValue(null);

        builder.Property(b => b.UpdatedById)
            .HasDefaultValue(null);

        builder.Property(b => b.CreatedAt)
            .IsRequired();

        builder.Property(b => b.CreatedById)
            .IsRequired();

        builder.Property(x => x.CreatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");
    }
}

