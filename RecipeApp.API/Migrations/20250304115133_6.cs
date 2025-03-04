using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RecipeApp.API.Migrations
{
    /// <inheritdoc />
    public partial class _6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "msg",
                table: "UserComments");

            migrationBuilder.AddColumn<string>(
                name: "message",
                table: "UserComments",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "message",
                table: "UserComments");

            migrationBuilder.AddColumn<int>(
                name: "msg",
                table: "UserComments",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
