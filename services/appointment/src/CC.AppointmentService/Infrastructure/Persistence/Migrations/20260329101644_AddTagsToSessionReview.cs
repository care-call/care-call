using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CC.AppointmentService.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddTagsToSessionReview : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid[]>(
                name: "tags",
                table: "session_reviews",
                type: "uuid[]",
                nullable: false,
                defaultValue: new Guid[0]);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "tags",
                table: "session_reviews");
        }
    }
}
