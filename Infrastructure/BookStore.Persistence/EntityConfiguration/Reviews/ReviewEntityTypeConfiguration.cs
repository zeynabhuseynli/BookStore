using BookStore.Domain.Entities.Enums;
using BookStore.Domain.Entities.Reviews;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookStore.Persistence.EntityConfiguration.Reviews;
public class ReviewEntityTypeConfiguration : IEntityTypeConfiguration<Review>
{
    public void Configure(EntityTypeBuilder<Review> builder)
    {
        builder.ToTable("Reviews");

        builder.HasKey(r => r.Id);

        builder.Property(r => r.Message)
               .IsRequired()
               .HasMaxLength(500);

        builder.Property(r => r.FromUserId)
               .IsRequired();

        builder.Property(r => r.ToUserId)
               .IsRequired(false);

        builder.Property(u => u.Rating)
            .HasConversion<int>();

        builder.HasOne(r => r.User)
               .WithMany(u => u.Reviews)
               .HasForeignKey(r => r.FromUserId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(r => r.User)
               .WithMany(u => u.Reviews)
               .HasForeignKey(r => r.ToUserId)
               .OnDelete(DeleteBehavior.Restrict)
               .IsRequired(false);
        builder.HasOne(r => r.Book)
               .WithMany(b => b.Reviews)
               .HasForeignKey(r => r.BookId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(r => new { r.FromUserId, r.ToUserId });

        builder.Property(x => x.CreatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");
    }
}

