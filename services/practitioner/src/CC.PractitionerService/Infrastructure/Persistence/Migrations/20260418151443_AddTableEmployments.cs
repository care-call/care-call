using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CC.PractitionerService.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddTableEmployments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "employments",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    practitioner_id = table.Column<Guid>(type: "uuid", nullable: false),
                    external_employment_key = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    period_from = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    period_to = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_employments", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "ix_employments_external_employment_key",
                table: "employments",
                column: "external_employment_key");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "employments");
        }
    }
}
