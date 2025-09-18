using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public sealed class IndicatorEntityTypeConfiguration : IEntityTypeConfiguration<Indicator>
{
    public void Configure(EntityTypeBuilder<Indicator> builder)
    {
        builder.ToTable("indicator");

        builder.HasKey(indicator => indicator.IndicatorId);

        builder.Property(indicator => indicator.IndicatorId)
            .HasMaxLength(128)
            .IsRequired();

        builder.Property(indicator => indicator.Name)
            .HasMaxLength(256)
            .IsRequired();

        builder.Property(indicator => indicator.Value)
            .IsRequired();

        builder.Property(indicator => indicator.CreatedAt)
            .HasColumnType("datetime(6)")
            .IsRequired();
    }
}
