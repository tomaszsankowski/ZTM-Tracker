using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VueZtmBackend.Domain.Entities;
using VueZtmBackend.Domain.ValueObjects;

namespace VueZtmBackend.Infrastructure.Persistence.Configurations;

public class UserStopConfiguration : IEntityTypeConfiguration<UserStop>
{
    public void Configure(EntityTypeBuilder<UserStop> builder)
    {
        builder.ToTable("UserStops");

        builder.HasKey(us => us.Id);

        builder.Property(us => us.Id)
            .ValueGeneratedNever();

        builder.Property(us => us.UserId)
            .IsRequired();

        builder.Property(us => us.StopId)
            .HasConversion(
                stopId => stopId.Value,
                value => new StopId(value))
            .IsRequired();

        builder.HasIndex(us => new { us.UserId, us.StopId })
            .IsUnique();

        builder.Property(us => us.CustomName)
            .HasMaxLength(100);

        builder.Property(us => us.CreatedAt)
            .IsRequired();

        builder.Property(us => us.UpdatedAt);
    }
}

