using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace server.src.Migrations
{
    /// <inheritdoc />
    public partial class UpdateEventSchemaToMatchDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "available_seats",
                table: "events");

            migrationBuilder.DropColumn(
                name: "description",
                table: "events");

            migrationBuilder.DropColumn(
                name: "end_date",
                table: "events");

            migrationBuilder.DropColumn(
                name: "location",
                table: "events");

            migrationBuilder.DropColumn(
                name: "status",
                table: "events");

            migrationBuilder.DropColumn(
                name: "ticket_price",
                table: "events");

            migrationBuilder.RenameColumn(
                name: "total_seats",
                table: "events",
                newName: "venue_capacity");

            migrationBuilder.RenameColumn(
                name: "start_date",
                table: "events",
                newName: "start_time");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "venue_capacity",
                table: "events",
                newName: "total_seats");

            migrationBuilder.RenameColumn(
                name: "start_time",
                table: "events",
                newName: "start_date");

            migrationBuilder.AddColumn<int>(
                name: "available_seats",
                table: "events",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "description",
                table: "events",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "end_date",
                table: "events",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "location",
                table: "events",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "status",
                table: "events",
                type: "text",
                nullable: false,
                defaultValue: "Published");

            migrationBuilder.AddColumn<decimal>(
                name: "ticket_price",
                table: "events",
                type: "numeric(10,2)",
                precision: 10,
                scale: 2,
                nullable: false,
                defaultValue: 0.00m);
        }
    }
}
