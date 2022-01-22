using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Meloman_clone.Migrations
{
    public partial class BookContext_AuthorPhoto_field : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "AuthorPhoto",
                table: "Books",
                type: "varbinary(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AuthorPhoto",
                table: "Books");
        }
    }
}
