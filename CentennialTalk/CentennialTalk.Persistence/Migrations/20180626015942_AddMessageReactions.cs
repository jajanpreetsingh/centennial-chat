using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace CentennialTalk.Persistence.Migrations
{
    public partial class AddMessageReactions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "RepliedMessageId",
                table: "Messages",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "SentDate",
                table: "Messages",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "JoiningTime",
                table: "GroupMembers",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "Reactions",
                columns: table => new
                {
                    MessageReactionId = table.Column<Guid>(nullable: false),
                    Sender = table.Column<string>(nullable: true),
                    ReactType = table.Column<int>(nullable: false),
                    MessageId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reactions", x => x.MessageReactionId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Reactions");

            migrationBuilder.DropColumn(
                name: "SentDate",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "JoiningTime",
                table: "GroupMembers");

            migrationBuilder.AlterColumn<string>(
                name: "RepliedMessageId",
                table: "Messages",
                nullable: true,
                oldClrType: typeof(Guid));
        }
    }
}