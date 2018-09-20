using Microsoft.EntityFrameworkCore.Migrations;

namespace CentennialTalk.Persistence.Migrations
{
    public partial class AddChatCodeToQuestion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ChatCode",
                table: "Questions",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ChatCode",
                table: "Polls",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChatCode",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "ChatCode",
                table: "Polls");
        }
    }
}
