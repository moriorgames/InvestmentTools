using Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public sealed class EntityFrameworkContext(DbContextOptions<EntityFrameworkContext> options) : DbContext(options)
{
    public DbSet<Indicator> Indicators => Set<Indicator>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Indicator>(eb =>
        {
            eb.ToTable("indicator");

            eb.HasKey(i => i.IndicatorId);
            eb.Property(i => i.IndicatorId).HasMaxLength(128).IsRequired();

            eb.Property(i => i.Name).HasMaxLength(256).IsRequired();
            eb.Property(i => i.Value).IsRequired();

            eb.Property(i => i.CreatedAt)
                .HasColumnType("datetime(6)")
                .IsRequired();
        });
    }
}