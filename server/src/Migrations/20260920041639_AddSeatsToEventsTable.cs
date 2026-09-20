using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace server.src.Migrations
{
    /// <inheritdoc />
    public partial class AddSeatsToEventsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "available_seats",
                table: "events",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "total_seats",
                table: "events",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "available_seats",
                table: "events");

            migrationBuilder.DropColumn(
                name: "total_seats",
                table: "events");
        }
    }
}
