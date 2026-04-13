namespace InterviewCoach.Models
{
    public class FeedbackResponse
    {
        public int Id { get; set; } 

        public float SituationScore { get; set; }
        public float TaskScore { get; set; }
        public float ActionScore { get; set; }
        public float ResultScore { get; set; }

        public string SituationFeedback { get; set; } = string.Empty;
        public string TaskFeedback { get; set; } = string.Empty;
        public string ActionFeedback { get; set; } = string.Empty;
        public string ResultFeedback { get; set; } = string.Empty;

        public string OverallFeedback { get; set; } = string.Empty;
    }
}