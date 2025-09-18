using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.Migrations;

public partial class InitialIndicator : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterDatabase()
            .Annotation("MySql:CharSet", "utf8mb4");

        migrationBuilder.CreateTable(
            name: "indicator",
            columns: table => new
            {
                indicator_id = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: false)
                    .Annotation("MySql:CharSet", "utf8mb4"),
                name = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: false)
                    .Annotation("MySql:CharSet", "utf8mb4"),
                value = table.Column<int>(type: "int", nullable: false),
                created_at = table.Column<DateTime>(type: "datetime(6)", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_indicator", x => x.indicator_id);
            })
            .Annotation("MySql:CharSet", "utf8mb4");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "indicator");
    }
}
