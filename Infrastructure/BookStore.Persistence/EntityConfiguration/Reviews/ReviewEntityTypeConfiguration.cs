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
            .HasDefaultValue(null);

        builder.Property(b => b.CreatedById)
            .HasDefaultValue(null);

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
    }
}

