using System.Net.Http;
using InterviewCoach.Models;
using InterviewCoach.Services;
using Xunit;
using Moq;

namespace InterviewCoach.Tests
{
    public class GeminiFeedbackServiceTests
    {
        [Fact]
        public void ParseGeminiResponse_ReturnsCorrectFeedbackResponse()
        {
            // Arrange
            var dummyHttpClient = new HttpClient();
            var service = new GeminiFeedbackService(dummyHttpClient);

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
            FeedbackResponse result = service.ParseGeminiResponse(mockGeminiJson);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(4.0f, result.SituationScore);
            Assert.Equal("Good context.", result.SituationFeedback);
            Assert.Equal("Solid answer overall.", result.OverallFeedback);
        }

        [Fact]
        public async Task GetRawGeminiResponseAsync_CallsApiAndReturnsString()
        {
            // Arrange
            string fakeResponseJson = "{\"candidates\": [{\"content\": {\"parts\": [{\"text\": \"fake generated feedback\"}]}}]}";
            
            // Use a delegating handler instead of mocking HttpMessageHandler directly
            var delegatingHandler = new FakeDelegatingHandler(fakeResponseJson);
            var fakeHttpClient = new HttpClient(delegatingHandler);
            var service = new GeminiFeedbackService(fakeHttpClient);

            var story = new Story { Situation = "Test Situation", Task = "Test Task", Action = "Test Action", Result = "Test Result" };

            // Act
            string result = await service.GetRawGeminiResponseAsync(story, "fake_api_key");

            // Assert
            Assert.Equal("fake generated feedback", result);
        }

        // Simple test delegating handler
        private class FakeDelegatingHandler : HttpMessageHandler
        {
            private readonly string _responseContent;

            public FakeDelegatingHandler(string responseContent)
            {
                _responseContent = responseContent;
            }

            protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                var response = new HttpResponseMessage
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Content = new StringContent(_responseContent)
                };
                return Task.FromResult(response);
            }
        }
    }
}
