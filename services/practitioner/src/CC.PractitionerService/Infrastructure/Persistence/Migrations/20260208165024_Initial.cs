using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CC.PractitionerService.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "practitioner_profiles",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    bio = table.Column<string>(type: "text", nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    patronymic = table.Column<string>(type: "text", nullable: true),
                    surname = table.Column<string>(type: "text", nullable: false),
                    photo_url = table.Column<string>(type: "text", nullable: true),
                    specializations_age_groups = table.Column<int[]>(type: "integer[]", nullable: false),
                    specializations_languages = table.Column<int[]>(type: "integer[]", nullable: false),
                    specializations_problem_areas = table.Column<int[]>(type: "integer[]", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_practitioner_profiles", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "work_schedules",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    practitioner_id = table.Column<Guid>(type: "uuid", nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false),
                    time_zone_id = table.Column<string>(type: "text", nullable: false),
                    session_duration = table.Column<TimeSpan>(type: "interval", nullable: false),
                    validity_period_from = table.Column<DateOnly>(type: "date", nullable: false),
                    validity_period_to = table.Column<DateOnly>(type: "date", nullable: true),
                    recurrences = table.Column<string>(type: "jsonb", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_work_schedules", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "practitioner_profiles");

            migrationBuilder.DropTable(
                name: "work_schedules");
        }
    }
}
