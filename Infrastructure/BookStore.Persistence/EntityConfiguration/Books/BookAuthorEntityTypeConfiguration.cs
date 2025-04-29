using BookStore.Domain.Entities.Authors;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookStore.Persistence.EntityConfiguration.Books;
public class BookAuthorEntityTypeConfiguration : IEntityTypeConfiguration<BookAuthor>
{
    public void Configure(EntityTypeBuilder<BookAuthor> builder)
    {
        builder.ToTable("book_authors");

        builder.HasKey(ba => new { ba.AuthorId, ba.BookId });

        builder.HasOne(ba => ba.Author)
               .WithMany(ba=>ba.Books)
               .HasForeignKey(ba => ba.AuthorId)
               .OnDelete(DeleteBehavior.Restrict); 

        builder.HasOne(ba => ba.Book)
               .WithMany(ba=>ba.Authors)
               .HasForeignKey(ba => ba.BookId)
               .OnDelete(DeleteBehavior.Restrict); 
    }
}

