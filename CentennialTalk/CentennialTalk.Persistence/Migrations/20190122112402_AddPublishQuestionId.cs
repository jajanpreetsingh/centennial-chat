using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CentennialTalk.Persistence.Migrations
{
    public partial class AddPublishQuestionId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "PublishedQuestionId",
                table: "Discussions",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PublishedQuestionId",
                table: "Discussions");
        }
    }
}
