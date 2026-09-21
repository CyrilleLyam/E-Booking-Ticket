using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace server.src.Migrations
{
    /// <inheritdoc />
    public partial class AddPriceAndStatusToEventsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "status",
                table: "events");

            migrationBuilder.DropColumn(
                name: "ticket_price",
                table: "events");
        }
    }
}
