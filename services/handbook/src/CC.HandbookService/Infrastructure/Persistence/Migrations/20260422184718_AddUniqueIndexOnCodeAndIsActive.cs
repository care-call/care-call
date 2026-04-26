using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CC.HandbookService.Migrations
{
    /// <inheritdoc />
    public partial class AddUniqueIndexOnCodeAndIsActive : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "is_active",
                table: "problem_areas",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "is_active",
                table: "languages",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "from_age",
                table: "age_groups",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "to_age",
                table: "age_groups",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "is_active",
                table: "age_groups",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_problem_areas_code",
                table: "problem_areas",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_languages_code",
                table: "languages",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_age_groups_code",
                table: "age_groups",
                column: "code",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_problem_areas_code",
                table: "problem_areas");

            migrationBuilder.DropIndex(
                name: "IX_languages_code",
                table: "languages");

            migrationBuilder.DropIndex(
                name: "IX_age_groups_code",
                table: "age_groups");

            migrationBuilder.DropColumn(
                name: "is_active",
                table: "problem_areas");

            migrationBuilder.DropColumn(
                name: "is_active",
                table: "languages");

            migrationBuilder.DropColumn(
                name: "from_age",
                table: "age_groups");

            migrationBuilder.DropColumn(
                name: "is_active",
                table: "age_groups");

            migrationBuilder.DropColumn(
                name: "to_age",
                table: "age_groups");
        }
    }
}
