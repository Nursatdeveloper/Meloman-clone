using Microsoft.EntityFrameworkCore.Migrations;

namespace Meloman_clone.Migrations
{
    public partial class BookContext_PeopleRated_field : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PeopleRated",
                table: "Books",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PeopleRated",
                table: "Books");
        }
    }
}
