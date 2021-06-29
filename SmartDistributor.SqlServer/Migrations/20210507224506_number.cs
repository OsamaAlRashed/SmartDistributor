using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartDistributor.SqlServer.Migrations
{
    public partial class number : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Number",
                table: "Categories",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Number",
                table: "Categories");
        }
    }
}
