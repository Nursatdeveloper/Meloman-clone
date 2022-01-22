using Microsoft.EntityFrameworkCore.Migrations;

namespace Meloman_clone.Migrations
{
    public partial class About_Citation_setup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "About",
                table: "BookDescriptions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Citation",
                table: "BookDescriptions",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "About",
                table: "BookDescriptions");

            migrationBuilder.DropColumn(
                name: "Citation",
                table: "BookDescriptions");
        }
    }
}
