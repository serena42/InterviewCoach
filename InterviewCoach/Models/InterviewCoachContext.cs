using Microsoft.EntityFrameworkCore;
using InterviewCoach.Models;

namespace InterviewCoach.Models
{
    public class InterviewCoachContext : DbContext
    {
        public InterviewCoachContext(DbContextOptions<InterviewCoachContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Story> Stories { get; set; }
        public DbSet<PracticeAttempt> PracticeAttempts { get; set; }
        public DbSet<StarRubric> StarRubrics { get; set; }
        public DbSet<FeedbackResponse> FeedbackResponse { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure relationships
            modelBuilder.Entity<Story>()
                .HasOne<User>()
                .WithMany(u => u.Stories)
                .HasForeignKey(s => s.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PracticeAttempt>()
                .HasOne(pa => pa.Story)
                .WithMany(s => s.PracticeAttempts)
                .HasForeignKey(pa => pa.StoryId)
                .OnDelete(DeleteBehavior.Cascade);

            // Seed initial rubric data
            modelBuilder.Entity<StarRubric>().HasData(
                new StarRubric
                {
                    Id = 1,
                    Section = "Situation",
                    Guidance = "Set the scene. Where were you, what was the context, why did it matter?",
                    GuidanceDetail = "A strong situation is specific, concise, and immediately relevant. It names the project, team, or context clearly. A weak situation is vague, too long, or could apply to anyone."
                },
                new StarRubric
                {
                    Id = 2,
                    Section = "Task",
                    Guidance = "What was your specific responsibility?",
                    GuidanceDetail = "A strong task statement clearly defines what YOU owned, not the team. It sets up why your actions mattered. A weak task is generic, uses 'we', or is hard to distinguish from the situation."
                },
                new StarRubric
                {
                    Id = 3,
                    Section = "Action",
                    Guidance = "What did YOU do? Use I not we.",
                    GuidanceDetail = "Strong actions are specific, sequential, and show judgment. They demonstrate the competency being assessed. Weak actions are vague, use 'we', or describe process without showing your thinking."
                },
                new StarRubric
                {
                    Id = 4,
                    Section = "Result",
                    Guidance = "What was the outcome? Quantify if possible.",
                    GuidanceDetail = "Strong results are quantified, tied directly to your actions, and include reflection or learning. Weak results are vague ('it went well'), unconnected to your actions, or missing entirely."
                }
            );
            
            // Seed questions
            SeedQuestions(modelBuilder);
        }

        private void SeedQuestions(ModelBuilder modelBuilder)
        {
            var questions = new List<Question>
            {
                new Question { Id = 1, QuestionId = "Q001", QuestionText = "Tell me about a time you had to influence someone over whom you had no authority.", Competency = "Leadership", QuestionType = "Behavioral", WhyAsked = "" },
                new Question { Id = 2, QuestionId = "Q002", QuestionText = "Describe a situation where you disagreed with a team decision. What did you do?", Competency = "Collaboration", QuestionType = "Behavioral", WhyAsked = "" },
                new Question { Id = 3, QuestionId = "Q003", QuestionText = "Give an example of a time you failed. What did you learn?", Competency = "Resilience", QuestionType = "Behavioral", WhyAsked = "" },
                new Question { Id = 4, QuestionId = "Q004", QuestionText = "Tell me about a time you had to make a decision with incomplete information.", Competency = "Judgment", QuestionType = "Behavioral", WhyAsked = "" },
                new Question { Id = 5, QuestionId = "Q005", QuestionText = "Describe a time when you had to deliver results under a tight deadline.", Competency = "Execution", QuestionType = "Behavioral", WhyAsked = "" },
                new Question { Id = 6, QuestionId = "N001", QuestionText = "Tell me about yourself.", Competency = "Self-Presentation", QuestionType = "Narrative", WhyAsked = "Interviewers use this as a warm-up to understand your background, how you communicate, and whether your story is relevant to the role." }
            };
            
            modelBuilder.Entity<Question>().HasData(questions);
        }
    }
}