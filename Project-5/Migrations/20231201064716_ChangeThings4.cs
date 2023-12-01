using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Project_5.Migrations
{
    public partial class ChangeThings4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeactive",
                table: "Cateogries",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeactive",
                table: "Cateogries");
        }
    }
}
