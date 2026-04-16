using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable


namespace InterviewCoach.Migrations
{
    /// <inheritdoc />
    public partial class CapturePendingMigrations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Questions",
                columns: new[] { "Id", "Competency", "QuestionId", "QuestionText", "QuestionType", "WhyAsked" },
                values: new object[,]
                {
                    { 1, "Leadership", "Q001", "Tell me about a time you had to influence someone over whom you had no authority.", "Behavioral", "" },
                    { 2, "Collaboration", "Q002", "Describe a situation where you disagreed with a team decision. What did you do?", "Behavioral", "" },
                    { 3, "Resilience", "Q003", "Give an example of a time you failed. What did you learn?", "Behavioral", "" },
                    { 4, "Judgment", "Q004", "Tell me about a time you had to make a decision with incomplete information.", "Behavioral", "" },
                    { 5, "Execution", "Q005", "Describe a time when you had to deliver results under a tight deadline.", "Behavioral", "" },
                    { 6, "Self-Presentation", "N001", "Tell me about yourself.", "Narrative", "Interviewers use this as a warm-up to understand your background, how you communicate, and whether your story is relevant to the role." }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: 6);
        }
    }
}
