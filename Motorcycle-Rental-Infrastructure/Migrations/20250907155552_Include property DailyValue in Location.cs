using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Motorcycle_Rental_Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class IncludepropertyDailyValueinLocation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DailyValue",
                table: "Locations",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DailyValue",
                table: "Locations");
        }
    }
}
