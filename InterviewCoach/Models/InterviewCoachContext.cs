using Microsoft.EntityFrameworkCore;

namespace InterviewCoach.Models
{
    public class InterviewCoachContext : DbContext
    {
        public InterviewCoachContext(DbContextOptions<InterviewCoachContext> options)
            : base(options)
        {
        }

        public DbSet<Story> Stories { get; set; }
        public DbSet<PracticeAttempt> PracticeAttempts { get; set; }
        public DbSet<StarRubric> StarRubrics { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

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

            // Configure relationships
            modelBuilder.Entity<PracticeAttempt>()
                .HasOne(pa => pa.Story)
                .WithMany(s => s.PracticeAttempts)
                .HasForeignKey(pa => pa.StoryId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}