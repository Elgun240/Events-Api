using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Project_5.Migrations
{
    public partial class CahngeColumnType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppUserMeasures_AspNetUsers_AppUserId1",
                table: "AppUserMeasures");

            migrationBuilder.DropIndex(
                name: "IX_AppUserMeasures_AppUserId1",
                table: "AppUserMeasures");

            migrationBuilder.DropColumn(
                name: "AppUserId1",
                table: "AppUserMeasures");

            migrationBuilder.AlterColumn<string>(
                name: "AppUserId",
                table: "AppUserMeasures",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AppUserMeasures_AppUserId",
                table: "AppUserMeasures",
                column: "AppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppUserMeasures_AspNetUsers_AppUserId",
                table: "AppUserMeasures",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppUserMeasures_AspNetUsers_AppUserId",
                table: "AppUserMeasures");

            migrationBuilder.DropIndex(
                name: "IX_AppUserMeasures_AppUserId",
                table: "AppUserMeasures");

            migrationBuilder.AlterColumn<int>(
                name: "AppUserId",
                table: "AppUserMeasures",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AppUserId1",
                table: "AppUserMeasures",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AppUserMeasures_AppUserId1",
                table: "AppUserMeasures",
                column: "AppUserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_AppUserMeasures_AspNetUsers_AppUserId1",
                table: "AppUserMeasures",
                column: "AppUserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
