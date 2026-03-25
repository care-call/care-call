using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CC.PractitionerService.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddColumnRatingInTablePractitionerProfile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "rating",
                table: "practitioner_profiles",
                type: "real",
                nullable: false,
                defaultValue: 0f);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "rating",
                table: "practitioner_profiles");
        }
    }
}
