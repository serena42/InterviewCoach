using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using InterviewCoach.Models;

namespace InterviewCoach.Services
{
    public class GeminiFeedbackService
    {
        private readonly HttpClient _httpClient;

        public GeminiFeedbackService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public FeedbackResponse ParseGeminiResponse(string jsonResponse)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var parsedResponse = JsonSerializer.Deserialize<FeedbackResponse>(jsonResponse, options);

            return parsedResponse ?? new FeedbackResponse();
        }

        public async Task<string> GetRawGeminiResponseAsync(Story story, string apiKey)
        {
            // Build the prompt for Gemini
            var prompt = $@"Evaluate this STAR interview response and provide scores and feedback.

Situation: {story.Situation}
Task: {story.Task}
Action: {story.Action}
Result: {story.Result}

Return ONLY valid JSON with this exact structure (no additional text):
{{
    ""SituationScore"": <float 0-5>,
    ""TaskScore"": <float 0-5>,
    ""ActionScore"": <float 0-5>,
    ""ResultScore"": <float 0-5>,
    ""SituationFeedback"": ""<feedback>"",
    ""TaskFeedback"": ""<feedback>"",
    ""ActionFeedback"": ""<feedback>"",
    ""ResultFeedback"": ""<feedback>"",
    ""OverallFeedback"": ""<feedback>"",
    ""Strengths"": ""<strengths>"",
    ""AreasForImprovement"": ""<areas for improvement>"",
    ""RecommendedFollowUpQuestions"": ""<follow-up questions>"",
    ""OverallScore"": <float 0-5>
}}";

            // Build the request body for Gemini API
            var requestBody = new
            {
                contents = new[]
                {
                    new
                    {
                        parts = new[]
                        {
                            new { text = prompt }
                        }
                    }
                }
            };

            var jsonContent = new StringContent(
                JsonSerializer.Serialize(requestBody),
                System.Text.Encoding.UTF8,
                "application/json"
            );

            // Make the API call
            var response = await _httpClient.PostAsync(
                $"https://generativelanguage.googleapis.com/v1beta/models/gemini-pro:generateContent?key={apiKey}",
                jsonContent
            );

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Gemini API request failed: {response.StatusCode}");
            }

            var responseContent = await response.Content.ReadAsStringAsync();

            // Parse the Gemini response to extract the text
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            using (var doc = JsonDocument.Parse(responseContent))
            {
                var root = doc.RootElement;
                var candidates = root.GetProperty("candidates");
                var firstCandidate = candidates[0];
                var content = firstCandidate.GetProperty("content");
                var parts = content.GetProperty("parts");
                var text = parts[0].GetProperty("text").GetString();

                return text ?? string.Empty;
            }
        }
    }
}