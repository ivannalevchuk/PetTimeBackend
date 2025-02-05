using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetTimeBackend.Migrations
{
    public partial class new_fields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "GoogleMapsId",
                table: "Places",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsPetFriendly",
                table: "Places",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GoogleMapsId",
                table: "Places");

            migrationBuilder.DropColumn(
                name: "IsPetFriendly",
                table: "Places");
        }
    }
}
