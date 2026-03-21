using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CC.AppointmentService.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddColumnUrlAppointments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "url",
                table: "appointments",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "url",
                table: "appointments");
        }
    }
}
