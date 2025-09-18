using Domain.Entity;
using Infrastructure.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public sealed class EntityFrameworkContext(DbContextOptions<EntityFrameworkContext> options) : DbContext(options)
{
    public DbSet<Indicator> Indicators => Set<Indicator>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseSnakeCaseNamingConvention();
        modelBuilder.HasCharSet("utf8mb4");

        modelBuilder.ApplyConfiguration(new IndicatorEntityTypeConfiguration());

        base.OnModelCreating(modelBuilder);
    }
}