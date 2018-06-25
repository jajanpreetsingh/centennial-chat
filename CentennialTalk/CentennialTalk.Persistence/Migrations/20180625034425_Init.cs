using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CentennialTalk.Persistence.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GroupMembers",
                columns: table => new
                {
                    GroupMemberId = table.Column<Guid>(nullable: false),
                    Username = table.Column<string>(maxLength: 255, nullable: false),
                    ConnectionId = table.Column<string>(nullable: true),
                    ChatCode = table.Column<string>(maxLength: 255, nullable: false),
                    IsConnected = table.Column<bool>(nullable: false),
                    DiscussionId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupMembers", x => x.Username);
                });

            migrationBuilder.CreateTable(
                name: "Discussions",
                columns: table => new
                {
                    DiscussionId = table.Column<Guid>(nullable: false),
                    ModeratorUsername = table.Column<string>(nullable: false),
                    Title = table.Column<string>(maxLength: 255, nullable: false),
                    DiscussionCode = table.Column<string>(maxLength: 8, nullable: false),
                    LastUpdated = table.Column<DateTime>(nullable: false),
                    IsLinkOpen = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Discussions", x => x.DiscussionId);
                    table.ForeignKey(
                        name: "FK_Discussions_GroupMembers_ModeratorUsername",
                        column: x => x.ModeratorUsername,
                        principalTable: "GroupMembers",
                        principalColumn: "Username",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    MessageId = table.Column<Guid>(nullable: false),
                    Content = table.Column<string>(maxLength: 255, nullable: false),
                    Sender = table.Column<string>(maxLength: 255, nullable: false),
                    ChatCode = table.Column<string>(maxLength: 8, nullable: false),
                    RepliedMessageId = table.Column<string>(nullable: true),
                    GroupMemberUsername = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.MessageId);
                    table.ForeignKey(
                        name: "FK_Messages_GroupMembers_GroupMemberUsername",
                        column: x => x.GroupMemberUsername,
                        principalTable: "GroupMembers",
                        principalColumn: "Username",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Discussions_ModeratorUsername",
                table: "Discussions",
                column: "ModeratorUsername");

            migrationBuilder.CreateIndex(
                name: "IX_GroupMembers_DiscussionId",
                table: "GroupMembers",
                column: "DiscussionId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_GroupMemberUsername",
                table: "Messages",
                column: "GroupMemberUsername");

            migrationBuilder.AddForeignKey(
                name: "FK_GroupMembers_Discussions_DiscussionId",
                table: "GroupMembers",
                column: "DiscussionId",
                principalTable: "Discussions",
                principalColumn: "DiscussionId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Discussions_GroupMembers_ModeratorUsername",
                table: "Discussions");

            migrationBuilder.DropTable(
                name: "Messages");

            migrationBuilder.DropTable(
                name: "GroupMembers");

            migrationBuilder.DropTable(
                name: "Discussions");
        }
    }
}
