using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartDistributor.SqlServer.Migrations
{
    public partial class a : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Lenght",
                table: "Products");

            migrationBuilder.AddColumn<int>(
                name: "Length",
                table: "Products",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Length",
                table: "Products");

            migrationBuilder.AddColumn<int>(
                name: "Lenght",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
