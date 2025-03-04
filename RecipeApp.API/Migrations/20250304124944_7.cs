using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RecipeApp.API.Migrations
{
    /// <inheritdoc />
    public partial class _7 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FavoriteList_Recipes_RecipeId",
                table: "FavoriteList");

            migrationBuilder.DropForeignKey(
                name: "FK_FavoriteList_Users_UserId",
                table: "FavoriteList");

            migrationBuilder.DropForeignKey(
                name: "FK_RecipeIngredients_Ingredient_IngredientId",
                table: "RecipeIngredients");

            migrationBuilder.DropForeignKey(
                name: "FK_RecipeIngredients_Unit_UnitId",
                table: "RecipeIngredients");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Unit",
                table: "Unit");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Ingredient",
                table: "Ingredient");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FavoriteList",
                table: "FavoriteList");

            migrationBuilder.RenameTable(
                name: "Unit",
                newName: "Units");

            migrationBuilder.RenameTable(
                name: "Ingredient",
                newName: "Ingredients");

            migrationBuilder.RenameTable(
                name: "FavoriteList",
                newName: "FavoriteLists");

            migrationBuilder.RenameIndex(
                name: "IX_FavoriteList_UserId",
                table: "FavoriteLists",
                newName: "IX_FavoriteLists_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_FavoriteList_RecipeId",
                table: "FavoriteLists",
                newName: "IX_FavoriteLists_RecipeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Units",
                table: "Units",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Ingredients",
                table: "Ingredients",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FavoriteLists",
                table: "FavoriteLists",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FavoriteLists_Recipes_RecipeId",
                table: "FavoriteLists",
                column: "RecipeId",
                principalTable: "Recipes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FavoriteLists_Users_UserId",
                table: "FavoriteLists",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RecipeIngredients_Ingredients_IngredientId",
                table: "RecipeIngredients",
                column: "IngredientId",
                principalTable: "Ingredients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RecipeIngredients_Units_UnitId",
                table: "RecipeIngredients",
                column: "UnitId",
                principalTable: "Units",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FavoriteLists_Recipes_RecipeId",
                table: "FavoriteLists");

            migrationBuilder.DropForeignKey(
                name: "FK_FavoriteLists_Users_UserId",
                table: "FavoriteLists");

            migrationBuilder.DropForeignKey(
                name: "FK_RecipeIngredients_Ingredients_IngredientId",
                table: "RecipeIngredients");

            migrationBuilder.DropForeignKey(
                name: "FK_RecipeIngredients_Units_UnitId",
                table: "RecipeIngredients");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Units",
                table: "Units");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Ingredients",
                table: "Ingredients");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FavoriteLists",
                table: "FavoriteLists");

            migrationBuilder.RenameTable(
                name: "Units",
                newName: "Unit");

            migrationBuilder.RenameTable(
                name: "Ingredients",
                newName: "Ingredient");

            migrationBuilder.RenameTable(
                name: "FavoriteLists",
                newName: "FavoriteList");

            migrationBuilder.RenameIndex(
                name: "IX_FavoriteLists_UserId",
                table: "FavoriteList",
                newName: "IX_FavoriteList_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_FavoriteLists_RecipeId",
                table: "FavoriteList",
                newName: "IX_FavoriteList_RecipeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Unit",
                table: "Unit",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Ingredient",
                table: "Ingredient",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FavoriteList",
                table: "FavoriteList",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FavoriteList_Recipes_RecipeId",
                table: "FavoriteList",
                column: "RecipeId",
                principalTable: "Recipes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FavoriteList_Users_UserId",
                table: "FavoriteList",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RecipeIngredients_Ingredient_IngredientId",
                table: "RecipeIngredients",
                column: "IngredientId",
                principalTable: "Ingredient",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RecipeIngredients_Unit_UnitId",
                table: "RecipeIngredients",
                column: "UnitId",
                principalTable: "Unit",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
