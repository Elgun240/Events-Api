using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Project_5.Migrations
{
    public partial class ChangeMeasureTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Accessibility",
                table: "Measures",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Measures",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "URL",
                table: "Measures",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Accessibility",
                table: "Measures");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Measures");

            migrationBuilder.DropColumn(
                name: "URL",
                table: "Measures");
        }
    }
}
