using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace CentennialTalk.Persistence.Migrations
{
    public partial class QuestionsAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Polls",
                columns: table => new
                {
                    IsPublished = table.Column<bool>(nullable: false),
                    QuestionId = table.Column<Guid>(nullable: false),
                    Type = table.Column<int>(nullable: false),
                    Content = table.Column<string>(nullable: true),
                    SelectMultiple = table.Column<bool>(nullable: false),
                    DiscussionId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Polls", x => x.QuestionId);
                    table.ForeignKey(
                        name: "FK_Polls_Discussions_DiscussionId",
                        column: x => x.DiscussionId,
                        principalTable: "Discussions",
                        principalColumn: "DiscussionId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Questions",
                columns: table => new
                {
                    IsPublished = table.Column<bool>(nullable: false),
                    QuestionId = table.Column<Guid>(nullable: false),
                    Type = table.Column<int>(nullable: false),
                    Content = table.Column<string>(nullable: true),
                    DiscussionId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Questions", x => x.QuestionId);
                    table.ForeignKey(
                        name: "FK_Questions_Discussions_DiscussionId",
                        column: x => x.DiscussionId,
                        principalTable: "Discussions",
                        principalColumn: "DiscussionId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "QuestionOption",
                columns: table => new
                {
                    OptionId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Text = table.Column<string>(nullable: true),
                    PollingQuestionQuestionId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionOption", x => x.OptionId);
                    table.ForeignKey(
                        name: "FK_QuestionOption_Polls_PollingQuestionQuestionId",
                        column: x => x.PollingQuestionQuestionId,
                        principalTable: "Polls",
                        principalColumn: "QuestionId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Polls_DiscussionId",
                table: "Polls",
                column: "DiscussionId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionOption_PollingQuestionQuestionId",
                table: "QuestionOption",
                column: "PollingQuestionQuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_Questions_DiscussionId",
                table: "Questions",
                column: "DiscussionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QuestionOption");

            migrationBuilder.DropTable(
                name: "Questions");

            migrationBuilder.DropTable(
                name: "Polls");
        }
    }
}