using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CachesJobTrackerApi.Migrations
{
    /// <inheritdoc />
    public partial class AddedJobDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "JobSalary",
                table: "Jobs",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "JobSource",
                table: "Jobs",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<List<string>>(
                name: "JobTags",
                table: "Jobs",
                type: "text[]",
                nullable: false,
                defaultValueSql: "'{}'");

            migrationBuilder.AddColumn<string>(
                name: "JobWorkMode",
                table: "Jobs",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "JobSalary",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "JobSource",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "JobTags",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "JobWorkMode",
                table: "Jobs");
        }
    }
}
