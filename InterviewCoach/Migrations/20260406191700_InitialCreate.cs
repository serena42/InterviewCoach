using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace InterviewCoach.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StarRubrics",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Section = table.Column<string>(type: "TEXT", nullable: false),
                    Guidance = table.Column<string>(type: "TEXT", nullable: false),
                    GuidanceDetail = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StarRubrics", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Stories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Situation = table.Column<string>(type: "TEXT", nullable: false),
                    Task = table.Column<string>(type: "TEXT", nullable: true),
                    Action = table.Column<string>(type: "TEXT", nullable: true),
                    Result = table.Column<string>(type: "TEXT", nullable: true),
                    SituationScore = table.Column<float>(type: "REAL", nullable: false),
                    TaskScore = table.Column<float>(type: "REAL", nullable: false),
                    ActionScore = table.Column<float>(type: "REAL", nullable: false),
                    ResultScore = table.Column<float>(type: "REAL", nullable: false),
                    AuthorName = table.Column<string>(type: "TEXT", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CoachingFeedback = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PracticeAttempts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    StoryId = table.Column<int>(type: "INTEGER", nullable: false),
                    AttemptDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    AttemptScore = table.Column<float>(type: "REAL", nullable: false),
                    Notes = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PracticeAttempts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PracticeAttempts_Stories_StoryId",
                        column: x => x.StoryId,
                        principalTable: "Stories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "StarRubrics",
                columns: new[] { "Id", "Guidance", "GuidanceDetail", "Section" },
                values: new object[,]
                {
                    { 1, "Set the scene. Where were you, what was the context, why did it matter?", "A strong situation is specific, concise, and immediately relevant. It names the project, team, or context clearly. A weak situation is vague, too long, or could apply to anyone.", "Situation" },
                    { 2, "What was your specific responsibility?", "A strong task statement clearly defines what YOU owned, not the team. It sets up why your actions mattered. A weak task is generic, uses 'we', or is hard to distinguish from the situation.", "Task" },
                    { 3, "What did YOU do? Use I not we.", "Strong actions are specific, sequential, and show judgment. They demonstrate the competency being assessed. Weak actions are vague, use 'we', or describe process without showing your thinking.", "Action" },
                    { 4, "What was the outcome? Quantify if possible.", "Strong results are quantified, tied directly to your actions, and include reflection or learning. Weak results are vague ('it went well'), unconnected to your actions, or missing entirely.", "Result" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_PracticeAttempts_StoryId",
                table: "PracticeAttempts",
                column: "StoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PracticeAttempts");

            migrationBuilder.DropTable(
                name: "StarRubrics");

            migrationBuilder.DropTable(
                name: "Stories");
        }
    }
}
