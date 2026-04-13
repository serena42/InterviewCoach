using InterviewCoach.Models;
using Xunit;

namespace InterviewCoach.Tests
{
    public class StoryTests
    {
        [Fact]
        public void OverallScore_CalculatesAverageCorrectly()
        {
            // Arrange - Set up the data we need for the test
            var story = new Story
            {
                SituationScore = 4.0f,
                TaskScore = 3.0f,
                ActionScore = 5.0f,
                ResultScore = 2.0f
            };

            // Act - Execute the logic we are testing
            var result = story.OverallScore;

            // Assert - Verify the result matches our expectations ( (4+3+5+2) / 4 = 3.5 )
            Assert.Equal(3.5f, result);
        }
    }
}
