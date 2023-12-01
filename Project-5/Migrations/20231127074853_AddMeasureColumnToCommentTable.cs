using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Project_5.Migrations
{
    public partial class AddMeasureColumnToCommentTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MeasureId",
                table: "Comments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Comments_MeasureId",
                table: "Comments",
                column: "MeasureId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Measures_MeasureId",
                table: "Comments",
                column: "MeasureId",
                principalTable: "Measures",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Measures_MeasureId",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Comments_MeasureId",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "MeasureId",
                table: "Comments");
        }
    }
}
