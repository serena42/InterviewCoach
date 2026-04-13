namespace InterviewCoach.Models
{
    public class PracticeAttempt
    {
        public int Id { get; set; }

        // Foreign key to the Story
        public int StoryId { get; set; }
        public Story? Story { get; set; }

        // When they practiced
        public DateTime AttemptDate { get; set; } = DateTime.UtcNow;

        // How well they did on "this" attempt
        public float AttemptScore { get; set; }

        // Notes for themselves
        public string? Notes { get; set; }
    }
}