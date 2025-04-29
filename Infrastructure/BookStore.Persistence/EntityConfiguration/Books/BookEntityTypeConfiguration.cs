using BookStore.Domain.Entities.Books;
using BookStore.Domain.Entities.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookStore.Persistence.EntityConfiguration.Books;
public class BookEntityTypeConfiguration : IEntityTypeConfiguration<Book>
{
    public void Configure(EntityTypeBuilder<Book> builder)
    {
        builder.ToTable("books");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Title)
               .IsRequired()
               .HasMaxLength(200);

        builder.Property(x => x.Description)
               .HasMaxLength(2000);

        builder.Property(x => x.Path)
               .IsRequired()
               .HasMaxLength(500);

        builder.Property(x => x.CoverPath)
               .HasMaxLength(500);

        builder.Property(u => u.Language)
              .IsRequired()
              .HasConversion<string>();

        builder.Property(u => u.Rating)
            .HasConversion<int>();

        builder.Property(u => u.AverageRating)
             .HasDefaultValue(0);

        builder.Property(u => u.Publisher)
               .IsRequired()
               .HasConversion<string>();

        builder.Property(x => x.PageCount)
               .IsRequired();

        builder.Property(x => x.ViewCount)
               .HasDefaultValue(0);

        builder.Property(x => x.IsDeleted)
               .HasDefaultValue(false);

        builder.Property(b => b.DeletedTime)
            .HasDefaultValue(null);

        builder.HasMany(b => b.Reviews)
           .WithOne(r => r.Book)
           .HasForeignKey(r => r.BookId)
           .OnDelete(DeleteBehavior.Cascade);

        builder.Property(x => x.CreatedAt)
       .HasDefaultValueSql("CURRENT_TIMESTAMP"); 

    }
}

