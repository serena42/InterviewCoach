using System;
using System.Collections.Generic;
using System.Text;

using InterviewCoach.Models;
using Xunit;
// using InterviewCoach.Services; // We will need this later!

namespace InterviewCoach.Tests
{
    public class GeminiFeedbackServiceTests
    {
        [Fact]
        public void ParseGeminiResponse_ReturnsCorrectFeedbackResponse()
        {
            // Arrange
            // 1. Instantiating a service that doesn't exist yet (Red phase of TDD!)
            var service = new GeminiFeedbackService();

            // 2. A simulated JSON response that we expect Gemini to return
            string mockGeminiJson = @"
            {
                ""SituationScore"": 4.0,
                ""TaskScore"": 3.5,
                ""ActionScore"": 5.0,
                ""ResultScore"": 4.5,
                ""SituationFeedback"": ""Good context."",
                ""TaskFeedback"": ""Clear goal."",
                ""ActionFeedback"": ""Great steps."",
                ""ResultFeedback"": ""Strong metrics."",
                ""OverallFeedback"": ""Solid answer overall.""
            }";

            // Act
            // 3. Calling a method that doesn't exist yet
            FeedbackResponse result = service.ParseGeminiResponse(mockGeminiJson);

            // Assert
            // 4. Verifying the parsed data matches what was in the JSON
            Assert.NotNull(result);
            Assert.Equal(4.0f, result.SituationScore);
            Assert.Equal("Good context.", result.SituationFeedback);
            Assert.Equal("Solid answer overall.", result.OverallFeedback);
        }
    }
}
