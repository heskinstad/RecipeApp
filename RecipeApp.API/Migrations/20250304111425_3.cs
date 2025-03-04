using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RecipeApp.API.Migrations
{
    /// <inheritdoc />
    public partial class _3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FavoriteList_User_UserId",
                table: "FavoriteList");

            migrationBuilder.DropForeignKey(
                name: "FK_Rating_User_UserId",
                table: "Rating");

            migrationBuilder.DropForeignKey(
                name: "FK_Recipes_User_UploaderId",
                table: "Recipes");

            migrationBuilder.DropForeignKey(
                name: "FK_UserComment_User_UserId",
                table: "UserComment");

            migrationBuilder.DropPrimaryKey(
                name: "PK_User",
                table: "User");

            migrationBuilder.RenameTable(
                name: "User",
                newName: "Users");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                table: "Users",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FavoriteList_Users_UserId",
                table: "FavoriteList",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Rating_Users_UserId",
                table: "Rating",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Recipes_Users_UploaderId",
                table: "Recipes",
                column: "UploaderId",
                principalTable: "Users",
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FavoriteList_Users_UserId",
                table: "FavoriteList");

            migrationBuilder.DropForeignKey(
                name: "FK_Rating_Users_UserId",
                table: "Rating");

            migrationBuilder.DropForeignKey(
                name: "FK_Recipes_Users_UploaderId",
                table: "Recipes");

            migrationBuilder.DropForeignKey(
                name: "FK_UserComment_Users_UserId",
                table: "UserComment");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "Users");

            migrationBuilder.RenameTable(
                name: "Users",
                newName: "User");

            migrationBuilder.AddPrimaryKey(
                name: "PK_User",
                table: "User",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FavoriteList_User_UserId",
                table: "FavoriteList",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Rating_User_UserId",
                table: "Rating",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Recipes_User_UploaderId",
                table: "Recipes",
                column: "UploaderId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserComment_User_UserId",
                table: "UserComment",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
