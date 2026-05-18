using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CC.TechSupportService.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddTicketRateLimitTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ticket_rate_limits",
                columns: table => new
                {
                    reporter_id = table.Column<Guid>(nullable: false),
                    count = table.Column<short>(nullable: false, defaultValue: (short)0),
                    created_at = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ticket_rate_limits", x => x.reporter_id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable("ticket_rate_limits");
        }
    }
}
