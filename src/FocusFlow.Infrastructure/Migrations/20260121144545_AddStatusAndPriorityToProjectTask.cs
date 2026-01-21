using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FocusFlow.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddStatusAndPriorityToProjectTask : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Priority",
                table: "Tasks",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Tasks",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: -1L,
                columns: new[] { "Created", "LastUpdated" },
                values: new object[] { new DateTime(2026, 1, 21, 14, 45, 45, 292, DateTimeKind.Utc).AddTicks(606), new DateTime(2026, 1, 21, 14, 45, 45, 292, DateTimeKind.Utc).AddTicks(1183) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Priority",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Tasks");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: -1L,
                columns: new[] { "Created", "LastUpdated" },
                values: new object[] { new DateTime(2026, 1, 21, 11, 12, 30, 312, DateTimeKind.Utc).AddTicks(3889), new DateTime(2026, 1, 21, 11, 12, 30, 312, DateTimeKind.Utc).AddTicks(4557) });
        }
    }
}
