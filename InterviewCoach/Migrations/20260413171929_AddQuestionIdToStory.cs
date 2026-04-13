using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InterviewCoach.Migrations
{
    /// <inheritdoc />
    public partial class AddQuestionIdToStory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "QuestionId",
                table: "Stories",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Stories",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "FeedbackResponse",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SituationScore = table.Column<float>(type: "REAL", nullable: false),
                    TaskScore = table.Column<float>(type: "REAL", nullable: false),
                    ActionScore = table.Column<float>(type: "REAL", nullable: false),
                    ResultScore = table.Column<float>(type: "REAL", nullable: false),
                    SituationFeedback = table.Column<string>(type: "TEXT", nullable: false),
                    TaskFeedback = table.Column<string>(type: "TEXT", nullable: false),
                    ActionFeedback = table.Column<string>(type: "TEXT", nullable: false),
                    ResultFeedback = table.Column<string>(type: "TEXT", nullable: false),
                    OverallFeedback = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeedbackResponse", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FeedbackResponse");

            migrationBuilder.DropColumn(
                name: "QuestionId",
                table: "Stories");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Stories");
        }
    }
}
