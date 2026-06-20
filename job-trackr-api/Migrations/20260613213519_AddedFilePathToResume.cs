using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CachesJobTrackerApi.Migrations
{
    /// <inheritdoc />
    public partial class AddedFilePathToResume : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ResumeFilePath",
                table: "Resumes",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ResumeFilePath",
                table: "Resumes");
        }
    }
}
