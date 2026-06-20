using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CachesJobTrackerApi.Migrations
{
    /// <inheritdoc />
    public partial class JobContactAsForeignKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "JobContactPerson",
                table: "Jobs");

            migrationBuilder.AddColumn<int>(
                name: "JobContactId",
                table: "Jobs",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_JobContactId",
                table: "Jobs",
                column: "JobContactId");

            migrationBuilder.AddForeignKey(
                name: "FK_Jobs_Contacts_JobContactId",
                table: "Jobs",
                column: "JobContactId",
                principalTable: "Contacts",
                principalColumn: "ContactId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Jobs_Contacts_JobContactId",
                table: "Jobs");

            migrationBuilder.DropIndex(
                name: "IX_Jobs_JobContactId",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "JobContactId",
                table: "Jobs");

            migrationBuilder.AddColumn<string>(
                name: "JobContactPerson",
                table: "Jobs",
                type: "text",
                nullable: true);
        }
    }
}
