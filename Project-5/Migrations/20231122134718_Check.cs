using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Project_5.Migrations
{
    public partial class Check : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_Measures_MeasureId",
                table: "Tickets");

            migrationBuilder.AlterColumn<int>(
                name: "MeasureId",
                table: "Tickets",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "Measures",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Adress",
                table: "Cateogries",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Company",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Company", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Measures_CompanyId",
                table: "Measures",
                column: "CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Measures_Company_CompanyId",
                table: "Measures",
                column: "CompanyId",
                principalTable: "Company",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_Measures_MeasureId",
                table: "Tickets",
                column: "MeasureId",
                principalTable: "Measures",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Measures_Company_CompanyId",
                table: "Measures");

            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_Measures_MeasureId",
                table: "Tickets");

            migrationBuilder.DropTable(
                name: "Company");

            migrationBuilder.DropIndex(
                name: "IX_Measures_CompanyId",
                table: "Measures");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "Measures");

            migrationBuilder.DropColumn(
                name: "Adress",
                table: "Cateogries");

            migrationBuilder.AlterColumn<int>(
                name: "MeasureId",
                table: "Tickets",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_Measures_MeasureId",
                table: "Tickets",
                column: "MeasureId",
                principalTable: "Measures",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
