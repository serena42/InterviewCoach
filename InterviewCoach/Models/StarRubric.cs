namespace InterviewCoach.Models
{
    public class StarRubric
    {
        public int Id { get; set; }

        public string Section { get; set; } = string.Empty; // "Situation", "Task", "Action", "Result"
        public string Guidance { get; set; } = string.Empty; // "Set the scene..."
        public string GuidanceDetail { get; set; } = string.Empty; // "A strong situation is specific..."
    }
}