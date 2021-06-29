using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartDistributor.SqlServer.Migrations
{
    public partial class ischoose : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsChoose",
                table: "Products",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsChoose",
                table: "Products");
        }
    }
}
