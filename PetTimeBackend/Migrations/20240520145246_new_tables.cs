using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetTimeBackend.Migrations
{
    public partial class new_tables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Articles_Breeds_BreedId",
                table: "Articles");

            migrationBuilder.RenameColumn(
                name: "BreedId",
                table: "Articles",
                newName: "CityId");

            migrationBuilder.RenameIndex(
                name: "IX_Articles_BreedId",
                table: "Articles",
                newName: "IX_Articles_CityId");

            migrationBuilder.CreateTable(
                name: "Country",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Country", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "City",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CountryId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_City", x => x.Id);
                    table.ForeignKey(
                        name: "FK_City_Country_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Country",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_City_CountryId",
                table: "City",
                column: "CountryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Articles_City_CityId",
                table: "Articles",
                column: "CityId",
                principalTable: "City",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Articles_City_CityId",
                table: "Articles");

            migrationBuilder.DropTable(
                name: "City");

            migrationBuilder.DropTable(
                name: "Country");

            migrationBuilder.RenameColumn(
                name: "CityId",
                table: "Articles",
                newName: "BreedId");

            migrationBuilder.RenameIndex(
                name: "IX_Articles_CityId",
                table: "Articles",
                newName: "IX_Articles_BreedId");

            migrationBuilder.AddForeignKey(
                name: "FK_Articles_Breeds_BreedId",
                table: "Articles",
                column: "BreedId",
                principalTable: "Breeds",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
