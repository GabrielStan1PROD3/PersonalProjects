using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "INGREDIENT_CATEGORY",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    Version = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_INGREDIENT_CATEGORY", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RECIPE_CATEGORY",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MealType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DishType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DietType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CountryType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LifestyleType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    Version = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RECIPE_CATEGORY", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "INGREDIENT",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    ImagePath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IngredientCategoryId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    Version = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    Nutrients = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_INGREDIENT", x => x.Id);
                    table.ForeignKey(
                        name: "FK_INGREDIENT_INGREDIENT_CATEGORY_IngredientCategoryId",
                        column: x => x.IngredientCategoryId,
                        principalTable: "INGREDIENT_CATEGORY",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RECIPE",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImagesPath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Dificulty = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RecipeCategoryId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    Version = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    Steps = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Times = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RECIPE", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RECIPE_RECIPE_CATEGORY_RecipeCategoryId",
                        column: x => x.RecipeCategoryId,
                        principalTable: "RECIPE_CATEGORY",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RECIPE_INGREDIENT",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QuantityInUnit = table.Column<double>(type: "float(10)", precision: 10, scale: 2, nullable: false),
                    Unit = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RecipeId = table.Column<int>(type: "int", nullable: false),
                    IngredientId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    Version = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RECIPE_INGREDIENT", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RECIPE_INGREDIENT_INGREDIENT_IngredientId",
                        column: x => x.IngredientId,
                        principalTable: "INGREDIENT",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RECIPE_INGREDIENT_RECIPE_RecipeId",
                        column: x => x.RecipeId,
                        principalTable: "RECIPE",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_INGREDIENT_IngredientCategoryId",
                table: "INGREDIENT",
                column: "IngredientCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_INGREDIENT_Name",
                table: "INGREDIENT",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RECIPE_RecipeCategoryId",
                table: "RECIPE",
                column: "RecipeCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_RECIPE_INGREDIENT_IngredientId",
                table: "RECIPE_INGREDIENT",
                column: "IngredientId");

            migrationBuilder.CreateIndex(
                name: "IX_RECIPE_INGREDIENT_RecipeId",
                table: "RECIPE_INGREDIENT",
                column: "RecipeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RECIPE_INGREDIENT");

            migrationBuilder.DropTable(
                name: "INGREDIENT");

            migrationBuilder.DropTable(
                name: "RECIPE");

            migrationBuilder.DropTable(
                name: "INGREDIENT_CATEGORY");

            migrationBuilder.DropTable(
                name: "RECIPE_CATEGORY");
        }
    }
}
