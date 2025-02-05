using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetTimeBackend.Migrations
{
    public partial class newType1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "Reports");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "UserId1",
                table: "Reports",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }
    }
}
