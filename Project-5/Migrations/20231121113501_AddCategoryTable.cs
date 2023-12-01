using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Project_5.Migrations
{
    public partial class AddCategoryTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "Measures",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Cateogries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cateogries", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Measures_CategoryId",
                table: "Measures",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Measures_Cateogries_CategoryId",
                table: "Measures",
                column: "CategoryId",
                principalTable: "Cateogries",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Measures_Cateogries_CategoryId",
                table: "Measures");

            migrationBuilder.DropTable(
                name: "Cateogries");

            migrationBuilder.DropIndex(
                name: "IX_Measures_CategoryId",
                table: "Measures");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Measures");
        }
    }
}
