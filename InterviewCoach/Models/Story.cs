namespace InterviewCoach.Models
{
    public class Story
    {
        public int? QuestionId { get; set; }
        public int? UserId { get; set; }
        public int Id { get; set; }

        public string Situation { get; set; } = string.Empty;
        public string? Task { get; set; }
        public string? Action { get; set; }
        public string? Result { get; set; }

        // Scores out of 5
        public float SituationScore { get; set; }
        public float TaskScore { get; set; }
        public float ActionScore { get; set; }
        public float ResultScore { get; set; }

        // Calculated property
        public float OverallScore => (SituationScore + TaskScore + ActionScore + ResultScore) / 4;

        // Who wrote it and when
        public string? AuthorName { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Coaching feedback from OpenAI
        public string? CoachingFeedback { get; set; }

        // Navigation: link to practice attempts
        public List<PracticeAttempt> PracticeAttempts { get; set; } = new();
    }
}