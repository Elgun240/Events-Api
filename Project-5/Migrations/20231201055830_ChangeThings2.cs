using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Project_5.Migrations
{
    public partial class ChangeThings2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.CreateTable(
                name: "AppUserMeasure",
                columns: table => new
                {
                    MeasuresId = table.Column<int>(type: "int", nullable: false),
                    UsersId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUserMeasure", x => new { x.MeasuresId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_AppUserMeasure_AspNetUsers_UsersId",
                        column: x => x.UsersId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppUserMeasure_Measures_MeasuresId",
                        column: x => x.MeasuresId,
                        principalTable: "Measures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppUserMeasure_UsersId",
                table: "AppUserMeasure",
                column: "UsersId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppUserMeasure");

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
    }
}
