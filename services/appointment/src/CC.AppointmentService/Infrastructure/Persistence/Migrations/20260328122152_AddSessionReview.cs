using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CC.AppointmentService.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddSessionReview : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "session_reviews",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    appointment_id = table.Column<Guid>(type: "uuid", nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false),
                    empathy_rating = table.Column<int>(type: "integer", nullable: false),
                    professionalism_rating = table.Column<int>(type: "integer", nullable: false),
                    comfort_rating = table.Column<int>(type: "integer", nullable: false),
                    review_comment = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_session_reviews", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "session_reviews");
        }
    }
}
