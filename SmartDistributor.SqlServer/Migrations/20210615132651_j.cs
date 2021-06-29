using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartDistributor.SqlServer.Migrations
{
    public partial class j : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "Sellers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "State",
                table: "Sellers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "City",
                table: "Sellers");

            migrationBuilder.DropColumn(
                name: "State",
                table: "Sellers");
        }
    }
}
