using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PoolTools.Player.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class PlayerStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "DateOfBirth",
                table: "Players",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "DATEADD(year, -18, CAST(GETDATE() as DATE))");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Players",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Players");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateOfBirth",
                table: "Players",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "DATEADD(year, -18, CAST(GETDATE() as DATE))",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");
        }
    }
}
