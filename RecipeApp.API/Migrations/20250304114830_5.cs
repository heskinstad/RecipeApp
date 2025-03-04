using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RecipeApp.API.Migrations
{
    /// <inheritdoc />
    public partial class _5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserComment_Recipes_RecipeId",
                table: "UserComment");

            migrationBuilder.DropForeignKey(
                name: "FK_UserComment_Users_UserId",
                table: "UserComment");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserComment",
                table: "UserComment");

            migrationBuilder.RenameTable(
                name: "UserComment",
                newName: "UserComments");

            migrationBuilder.RenameIndex(
                name: "IX_UserComment_UserId",
                table: "UserComments",
                newName: "IX_UserComments_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserComment_RecipeId",
                table: "UserComments",
                newName: "IX_UserComments_RecipeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserComments",
                table: "UserComments",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserComments_Recipes_RecipeId",
                table: "UserComments",
                column: "RecipeId",
                principalTable: "Recipes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserComments_Users_UserId",
                table: "UserComments",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserComments_Recipes_RecipeId",
                table: "UserComments");

            migrationBuilder.DropForeignKey(
                name: "FK_UserComments_Users_UserId",
                table: "UserComments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserComments",
                table: "UserComments");

            migrationBuilder.RenameTable(
                name: "UserComments",
                newName: "UserComment");

            migrationBuilder.RenameIndex(
                name: "IX_UserComments_UserId",
                table: "UserComment",
                newName: "IX_UserComment_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserComments_RecipeId",
                table: "UserComment",
                newName: "IX_UserComment_RecipeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserComment",
                table: "UserComment",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserComment_Recipes_RecipeId",
                table: "UserComment",
                column: "RecipeId",
                principalTable: "Recipes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserComment_Users_UserId",
                table: "UserComment",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
