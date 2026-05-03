using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CC.NotificationService.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdatesTemplates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "create_at",
                table: "templates",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "create_at",
                table: "template_versions",
                newName: "created_at");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "templates",
                newName: "create_at");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "template_versions",
                newName: "create_at");
        }
    }
}
