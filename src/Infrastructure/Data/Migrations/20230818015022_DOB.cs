using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PoolTools.Player.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class DOB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateOfBirth",
                table: "Players",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "DATEADD(year, -18, CAST(GETDATE() as DATE))");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateOfBirth",
                table: "Players");
        }
    }
}
