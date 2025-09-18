using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.Migrations;

[DbContext(typeof(EntityFrameworkContext))]
partial class EntityFrameworkContextModelSnapshot : ModelSnapshot
{
    protected override void BuildModel(ModelBuilder modelBuilder)
    {
#pragma warning disable 612, 618
        modelBuilder
            .HasAnnotation("ProductVersion", "9.0.9")
            .HasAnnotation("Relational:MaxIdentifierLength", 64)
            .HasAnnotation("MySql:CharSet", "utf8mb4");

        modelBuilder.HasCharSet("utf8mb4");

        modelBuilder.Entity("Domain.Entity.Indicator", b =>
        {
            b.Property<string>("IndicatorId")
                .HasMaxLength(128)
                .HasColumnType("varchar(128)")
                .HasColumnName("indicator_id")
                .HasCharSet("utf8mb4");

            b.Property<DateTime>("CreatedAt")
                .HasColumnType("datetime(6)")
                .HasColumnName("created_at");

            b.Property<string>("Name")
                .HasMaxLength(256)
                .HasColumnType("varchar(256)")
                .HasColumnName("name")
                .HasCharSet("utf8mb4");

            b.Property<int>("Value")
                .HasColumnType("int")
                .HasColumnName("value");

            b.HasKey("IndicatorId");

            b.ToTable("indicator")
                .HasCharSet("utf8mb4");
        });
#pragma warning restore 612, 618
    }
}
