using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Project_5.Migrations
{
    public partial class AddAppUserMeasuresTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppUserMeasures",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppUserId1 = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    AppUserId = table.Column<int>(type: "int", nullable: true),
                    MeasureId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUserMeasures", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppUserMeasures_AspNetUsers_AppUserId1",
                        column: x => x.AppUserId1,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppUserMeasures_Measures_MeasureId",
                        column: x => x.MeasureId,
                        principalTable: "Measures",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppUserMeasures_AppUserId1",
                table: "AppUserMeasures",
                column: "AppUserId1");

            migrationBuilder.CreateIndex(
                name: "IX_AppUserMeasures_MeasureId",
                table: "AppUserMeasures",
                column: "MeasureId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppUserMeasures");
        }
    }
}
