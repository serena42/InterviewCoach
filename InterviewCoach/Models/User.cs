using System;

namespace InterviewCoach.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation: user's stories
        public List<Story> Stories { get; set; } = new();
    }
}