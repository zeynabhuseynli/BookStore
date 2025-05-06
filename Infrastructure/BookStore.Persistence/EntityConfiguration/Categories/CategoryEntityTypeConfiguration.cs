using BookStore.Domain.Entities.Categories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookStore.Persistence.EntityConfiguration.Categories;
public class CategoryEntityTypeConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable("categories");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Name)
               .IsRequired()
               .HasMaxLength(100);

        builder.Property(c => c.IsDeleted)
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

        builder.HasMany(c => c.BookCategories)
            .WithOne(bc => bc.Category)
            .HasForeignKey(bc => bc.CategoryId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(x => x.CreatedAt)
    .HasDefaultValueSql("CURRENT_TIMESTAMP");
    }
}

