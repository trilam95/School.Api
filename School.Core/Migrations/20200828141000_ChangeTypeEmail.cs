using Microsoft.EntityFrameworkCore.Migrations;

namespace School.Core.Migrations
{
    public partial class ChangeTypeEmail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "User",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Email",
                table: "User",
                type: "int",
                nullable: false,
                oldClrType: typeof(string));
        }
    }
}
