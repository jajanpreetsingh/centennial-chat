using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace CentennialTalk.Persistence.Migrations
{
    public partial class RemoveCircularDependency : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Discussions_GroupMembers_ModeratorGroupMemberId",
                table: "Discussions");

            migrationBuilder.DropIndex(
                name: "IX_Discussions_ModeratorGroupMemberId",
                table: "Discussions");

            migrationBuilder.DropColumn(
                name: "ModeratorGroupMemberId",
                table: "Discussions");

            migrationBuilder.AddColumn<bool>(
                name: "IsModerator",
                table: "GroupMembers",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsModerator",
                table: "GroupMembers");

            migrationBuilder.AddColumn<Guid>(
                name: "ModeratorGroupMemberId",
                table: "Discussions",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

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
        }
    }
}