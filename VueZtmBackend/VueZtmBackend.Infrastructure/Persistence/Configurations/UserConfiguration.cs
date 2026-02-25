using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VueZtmBackend.Domain.Entities;
using VueZtmBackend.Domain.ValueObjects;

namespace VueZtmBackend.Infrastructure.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

        builder.HasKey(u => u.Id);

        builder.Property(u => u.Id)
            .ValueGeneratedNever();

        builder.Property(u => u.Login)
            .HasConversion(
                login => login.Value,
                value => new Login(value))
            .HasMaxLength(50)
            .IsRequired();

        builder.HasIndex(u => u.Login)
            .IsUnique();

        builder.Property(u => u.PasswordHash)
            .HasConversion(
                hash => hash.Value,
                value => new PasswordHash(value))
            .HasMaxLength(255)
            .IsRequired();

        builder.OwnsOne(u => u.RefreshToken, rt =>
        {
            rt.Property(r => r.Token)
                .HasColumnName("RefreshToken")
                .HasMaxLength(255);
            rt.Property(r => r.ExpiresAt)
                .HasColumnName("RefreshTokenExpiresAt");
            rt.Property(r => r.CreatedAt)
                .HasColumnName("RefreshTokenCreatedAt");
        });

        builder.Property(u => u.CreatedAt)
            .IsRequired();

        builder.Property(u => u.UpdatedAt);

        builder.HasMany(u => u.UserStops)
            .WithOne(us => us.User)
            .HasForeignKey(us => us.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

