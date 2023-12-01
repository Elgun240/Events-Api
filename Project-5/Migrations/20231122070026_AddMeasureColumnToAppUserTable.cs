using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Project_5.Migrations
{
    public partial class AddMeasureColumnToAppUserTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MeasureId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_MeasureId",
                table: "AspNetUsers",
                column: "MeasureId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Measures_MeasureId",
                table: "AspNetUsers",
                column: "MeasureId",
                principalTable: "Measures",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Measures_MeasureId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_MeasureId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "MeasureId",
                table: "AspNetUsers");
        }
    }
}
