using BookStore.Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookStore.Persistence.EntityConfiguration.Users;
public class UserEntityTypeConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");

        builder.HasKey(u => u.Id);

        builder.Property(u => u.FirstName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(u => u.LastName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(u => u.Email)
            .IsRequired()
            .HasMaxLength(200);

        builder.HasIndex(u => u.Email)
            .IsUnique();

        builder.Property(u => u.Gender)
            .IsRequired()
            .HasConversion<string>();

        builder.Property(u => u.Role)
            .IsRequired()
            .HasConversion<string>();

        builder.Property(u => u.BirthDay)
            .IsRequired();

        builder.Property(u => u.LoginCount)
            .HasDefaultValue(0);

        builder.Property(u => u.IsDeleted)
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

        builder.Property(u => u.IsActivated)
            .HasDefaultValue(false);

        builder.Property(u => u.RefreshToken)
            .HasDefaultValue(null);

        builder.Property(u => u.PasswordHash)
            .IsRequired();

        builder.Property(u => u.PasswordResetOtp)
            .HasDefaultValue(null);

        builder.Property(u => u.PasswordResetOtpDate)
            .HasDefaultValue(null);

        builder.Property(u => u.ResetPasswordDate)
            .HasDefaultValue(null);

        builder.HasMany(u => u.Reviews)
            .WithOne(r => r.User)
            .HasForeignKey(r => r.FromUserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

