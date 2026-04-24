using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CC.TechSupportService.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "tech_support");

            migrationBuilder.CreateTable(
                name: "ticket_histories",
                schema: "tech_support",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    ticket_id = table.Column<Guid>(type: "uuid", nullable: false),
                    actor_id = table.Column<Guid>(type: "uuid", nullable: false),
                    action_type = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    old_value = table.Column<string>(type: "jsonb", nullable: true),
                    new_value = table.Column<string>(type: "jsonb", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_ticket_histories", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tickets",
                schema: "tech_support",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    number = table.Column<int>(type: "integer", nullable: false),
                    assignee_id = table.Column<Guid>(type: "uuid", nullable: true),
                    subject = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    description = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    ticket_category = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    status = table.Column<string>(type: "text", nullable: false),
                    first_responded_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    sla_first_response_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    last_user_responded_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    resolved_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    closed_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    csat_rating = table.Column<byte>(type: "smallint", nullable: true),
                    csat_comment = table.Column<string>(type: "text", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    related_entity_related_entity_id = table.Column<Guid>(type: "uuid", nullable: true),
                    related_entity_related_type = table.Column<string>(type: "text", nullable: true),
                    reporter_reporter_id = table.Column<Guid>(type: "uuid", nullable: false),
                    reporter_reporter_type = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_tickets", x => x.id);
                    table.UniqueConstraint("ak_tickets_number", x => x.number);
                });

            migrationBuilder.CreateTable(
                name: "users_request",
                schema: "tech_support",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    amount_of_request_for_last_hour = table.Column<byte>(type: "smallint", nullable: false),
                    last_request_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_users_request", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "ticket_attachments",
                schema: "tech_support",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    ticket_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_ticket_attachments", x => x.id);
                    table.ForeignKey(
                        name: "fk_ticket_attachments_tickets_ticket_id",
                        column: x => x.ticket_id,
                        principalSchema: "tech_support",
                        principalTable: "tickets",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ticket_comments",
                schema: "tech_support",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    ticket_id = table.Column<Guid>(type: "uuid", nullable: false),
                    is_internal = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    author_id = table.Column<Guid>(type: "uuid", nullable: false),
                    author_type = table.Column<string>(type: "text", nullable: false),
                    body_body = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_ticket_comments", x => x.id);
                    table.ForeignKey(
                        name: "fk_ticket_comments_tickets_ticket_id",
                        column: x => x.ticket_id,
                        principalSchema: "tech_support",
                        principalTable: "tickets",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "files_details",
                schema: "tech_support",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    attachment_id = table.Column<Guid>(type: "uuid", nullable: false),
                    file_name = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    file_path = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    content_type = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    content_size = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_files_details", x => x.id);
                    table.ForeignKey(
                        name: "fk_files_details_ticket_attachments_attachment_id",
                        column: x => x.attachment_id,
                        principalSchema: "tech_support",
                        principalTable: "ticket_attachments",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_files_details_attachment_id",
                schema: "tech_support",
                table: "files_details",
                column: "attachment_id");

            migrationBuilder.CreateIndex(
                name: "ix_ticket_attachments_ticket_id",
                schema: "tech_support",
                table: "ticket_attachments",
                column: "ticket_id");

            migrationBuilder.CreateIndex(
                name: "ix_ticket_comments_ticket_id",
                schema: "tech_support",
                table: "ticket_comments",
                column: "ticket_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "files_details",
                schema: "tech_support");

            migrationBuilder.DropTable(
                name: "ticket_comments",
                schema: "tech_support");

            migrationBuilder.DropTable(
                name: "ticket_histories",
                schema: "tech_support");

            migrationBuilder.DropTable(
                name: "users_request",
                schema: "tech_support");

            migrationBuilder.DropTable(
                name: "ticket_attachments",
                schema: "tech_support");

            migrationBuilder.DropTable(
                name: "tickets",
                schema: "tech_support");
        }
    }
}
