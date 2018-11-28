using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CentennialTalk.Persistence.Migrations
{
    public partial class AddDatesToQuestion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsLinkOpen",
                table: "Discussions");

            migrationBuilder.AddColumn<DateTime>(
                name: "ArchiveDate",
                table: "Questions",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsArchived",
                table: "Questions",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "PublishDate",
                table: "Questions",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ArchiveDate",
                table: "Polls",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsArchived",
                table: "Polls",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "PublishDate",
                table: "Polls",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ArchiveDate",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "IsArchived",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "PublishDate",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "ArchiveDate",
                table: "Polls");

            migrationBuilder.DropColumn(
                name: "IsArchived",
                table: "Polls");

            migrationBuilder.DropColumn(
                name: "PublishDate",
                table: "Polls");

            migrationBuilder.AddColumn<bool>(
                name: "IsLinkOpen",
                table: "Discussions",
                nullable: false,
                defaultValue: false);
        }
    }
}
