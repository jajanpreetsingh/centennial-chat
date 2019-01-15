using Microsoft.EntityFrameworkCore.Migrations;

namespace CentennialTalk.Persistence.Migrations
{
    public partial class AddCreatorId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatorId",
                table: "Discussions",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "Discussions");
        }
    }
}