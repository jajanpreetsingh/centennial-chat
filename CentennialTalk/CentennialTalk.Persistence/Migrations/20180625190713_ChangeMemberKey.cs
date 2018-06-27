using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace CentennialTalk.Persistence.Migrations
{
    public partial class ChangeMemberKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Discussions_GroupMembers_ModeratorUsername",
                table: "Discussions");

            migrationBuilder.DropForeignKey(
                name: "FK_Messages_GroupMembers_GroupMemberUsername",
                table: "Messages");

            migrationBuilder.DropIndex(
                name: "IX_Messages_GroupMemberUsername",
                table: "Messages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GroupMembers",
                table: "GroupMembers");

            migrationBuilder.DropIndex(
                name: "IX_Discussions_ModeratorUsername",
                table: "Discussions");

            migrationBuilder.DropColumn(
                name: "GroupMemberUsername",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "ModeratorUsername",
                table: "Discussions");

            migrationBuilder.AddColumn<Guid>(
                name: "GroupMemberId",
                table: "Messages",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ModeratorGroupMemberId",
                table: "Discussions",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_GroupMembers",
                table: "GroupMembers",
                column: "GroupMemberId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_GroupMemberId",
                table: "Messages",
                column: "GroupMemberId");

            migrationBuilder.CreateIndex(
                name: "IX_Discussions_ModeratorGroupMemberId",
                table: "Discussions",
                column: "ModeratorGroupMemberId");

            migrationBuilder.AddForeignKey(
                name: "FK_Discussions_GroupMembers_ModeratorGroupMemberId",
                table: "Discussions",
                column: "ModeratorGroupMemberId",
                principalTable: "GroupMembers",
                principalColumn: "GroupMemberId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_GroupMembers_GroupMemberId",
                table: "Messages",
                column: "GroupMemberId",
                principalTable: "GroupMembers",
                principalColumn: "GroupMemberId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Discussions_GroupMembers_ModeratorGroupMemberId",
                table: "Discussions");

            migrationBuilder.DropForeignKey(
                name: "FK_Messages_GroupMembers_GroupMemberId",
                table: "Messages");

            migrationBuilder.DropIndex(
                name: "IX_Messages_GroupMemberId",
                table: "Messages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GroupMembers",
                table: "GroupMembers");

            migrationBuilder.DropIndex(
                name: "IX_Discussions_ModeratorGroupMemberId",
                table: "Discussions");

            migrationBuilder.DropColumn(
                name: "GroupMemberId",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "ModeratorGroupMemberId",
                table: "Discussions");

            migrationBuilder.AddColumn<string>(
                name: "GroupMemberUsername",
                table: "Messages",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModeratorUsername",
                table: "Discussions",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GroupMembers",
                table: "GroupMembers",
                column: "Username");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_GroupMemberUsername",
                table: "Messages",
                column: "GroupMemberUsername");

            migrationBuilder.CreateIndex(
                name: "IX_Discussions_ModeratorUsername",
                table: "Discussions",
                column: "ModeratorUsername");

            migrationBuilder.AddForeignKey(
                name: "FK_Discussions_GroupMembers_ModeratorUsername",
                table: "Discussions",
                column: "ModeratorUsername",
                principalTable: "GroupMembers",
                principalColumn: "Username",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_GroupMembers_GroupMemberUsername",
                table: "Messages",
                column: "GroupMemberUsername",
                principalTable: "GroupMembers",
                principalColumn: "Username",
                onDelete: ReferentialAction.Restrict);
        }
    }
}