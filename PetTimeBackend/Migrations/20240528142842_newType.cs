using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetTimeBackend.Migrations
{
    public partial class newType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reports_Users_UserId1",
                table: "Reports");

            migrationBuilder.DropIndex(
                name: "IX_Reports_UserId1",
                table: "Reports");

            migrationBuilder.AlterColumn<long>(
                name: "UserId",
                table: "Reports",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_UserId",
                table: "Reports",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_Users_UserId",
                table: "Reports",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reports_Users_UserId",
                table: "Reports");

            migrationBuilder.DropIndex(
                name: "IX_Reports_UserId",
                table: "Reports");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Reports",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_UserId1",
                table: "Reports",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_Users_UserId1",
                table: "Reports",
                column: "UserId1",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
