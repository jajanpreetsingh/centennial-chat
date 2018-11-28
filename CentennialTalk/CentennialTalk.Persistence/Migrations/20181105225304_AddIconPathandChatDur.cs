using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CentennialTalk.Persistence.Migrations
{
    public partial class AddIconPathandChatDur : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IconPath",
                table: "GroupMembers",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ActivationDate",
                table: "Discussions",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IconPath",
                table: "GroupMembers");

            migrationBuilder.DropColumn(
                name: "ActivationDate",
                table: "Discussions");
        }
    }
}
