namespace InterviewCoach.Models
{
    public class Question
    {
        public int Id { get; set; }
        public string QuestionId { get; set; } = string.Empty; // e.g., Q001, N001
        public string QuestionText { get; set; } = string.Empty;
        public string Competency { get; set; } = string.Empty;
        public string QuestionType { get; set; } = string.Empty; // "Behavioral" or "Narrative"
        public string WhyAsked { get; set; } = string.Empty;
    }
}