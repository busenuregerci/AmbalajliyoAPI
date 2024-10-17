using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ambalajliyo.DAL.Migrations.SqlServerLogMigrations
{
    /// <inheritdoc />
    public partial class log2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Timestamp",
                table: "Logs",
                newName: "TimeStamp");

            migrationBuilder.RenameColumn(
                name: "RenderedMessage",
                table: "Logs",
                newName: "Message");

            migrationBuilder.AlterColumn<DateTime>(
                name: "TimeStamp",
                table: "Logs",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TimeStamp",
                table: "Logs",
                newName: "Timestamp");

            migrationBuilder.RenameColumn(
                name: "Message",
                table: "Logs",
                newName: "RenderedMessage");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "Timestamp",
                table: "Logs",
                type: "datetimeoffset",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");
        }
    }
}
