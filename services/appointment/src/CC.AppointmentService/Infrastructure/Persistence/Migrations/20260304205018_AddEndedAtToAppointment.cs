using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CC.AppointmentService.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddEndedAtToAppointment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ended_at",
                table: "appointments",
                type: "timestamp without time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ended_at",
                table: "appointments");
        }
    }
}
