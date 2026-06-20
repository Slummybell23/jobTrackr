using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CachesJobTrackerApi.Migrations
{
    /// <inheritdoc />
    public partial class AddedFollowUpReminder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FollowUpReminders",
                columns: table => new
                {
                    FollowUpReminderId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    JobId = table.Column<int>(type: "integer", nullable: false),
                    FollowUpReminderTitle = table.Column<string>(type: "text", nullable: false),
                    FollowUpReminderNotes = table.Column<string>(type: "text", nullable: true),
                    FollowUpReminderDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsCompleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FollowUpReminders", x => x.FollowUpReminderId);
                    table.ForeignKey(
                        name: "FK_FollowUpReminders_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FollowUpReminders_Jobs_JobId",
                        column: x => x.JobId,
                        principalTable: "Jobs",
                        principalColumn: "JobId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FollowUpReminders_JobId",
                table: "FollowUpReminders",
                column: "JobId");

            migrationBuilder.CreateIndex(
                name: "IX_FollowUpReminders_UserId",
                table: "FollowUpReminders",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FollowUpReminders");
        }
    }
}
