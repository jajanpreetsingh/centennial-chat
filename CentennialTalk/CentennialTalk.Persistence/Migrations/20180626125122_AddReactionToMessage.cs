using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace CentennialTalk.Persistence.Migrations
{
    public partial class AddReactionToMessage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "MessageId",
                table: "Reactions",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.CreateIndex(
                name: "IX_Reactions_MessageId",
                table: "Reactions",
                column: "MessageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reactions_Messages_MessageId",
                table: "Reactions",
                column: "MessageId",
                principalTable: "Messages",
                principalColumn: "MessageId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reactions_Messages_MessageId",
                table: "Reactions");

            migrationBuilder.DropIndex(
                name: "IX_Reactions_MessageId",
                table: "Reactions");

            migrationBuilder.AlterColumn<Guid>(
                name: "MessageId",
                table: "Reactions",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);
        }
    }
}