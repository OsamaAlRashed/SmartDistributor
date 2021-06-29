using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartDistributor.SqlServer.Migrations
{
    public partial class dsa : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ClusterId",
                table: "Sellers",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClusterId",
                table: "Sellers");
        }
    }
}
