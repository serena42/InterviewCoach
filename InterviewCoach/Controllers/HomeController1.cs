using Microsoft.AspNetCore.Mvc;
using InterviewCoach.Models;
using OpenAI;

namespace InterviewCoach.Controllers
{
    public class CoachController : Controller
    {
        private readonly InterviewCoachContext _context;
        private readonly OpenAIClient _openaiClient;

        public CoachController(InterviewCoachContext context)
        {
            _context = context;
            // Initialize OpenAI client with your API key from environment
            var apiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY")
                ?? throw new InvalidOperationException("OPENAI_API_KEY environment variable not set");
            _openaiClient = new OpenAIClient(apiKey);
        }

        // POST: Coach/GetFeedback
        [HttpPost]
        public async Task<IActionResult> GetFeedback(int storyId, string element)
        {
            try
            {
                var story = await _context.Stories.FindAsync(storyId);
                if (story == null)
                {
                    return BadRequest("Story not found");
                }

                // Build the prompt for OpenAI
                var prompt = BuildCoachingPrompt(story, element);

                // Call OpenAI API
                var chatCompletion = await _openaiClient.GetChatClient("gpt-4o-mini").CreateChatCompletionAsync(
                    new OpenAI.Chat.ChatMessage[]
                    {
                        new OpenAI.Chat.SystemChatMessage("You are an expert interview coach helping candidates improve their STAR stories for technical interviews."),
                        new OpenAI.Chat.UserChatMessage(prompt)
                    }
                );

                var feedbackText = chatCompletion.Value.Content[0].Text;

                // Save feedback to the story
                story.CoachingFeedback = feedbackText;
                _context.Update(story);
                await _context.SaveChangesAsync();

                return Json(new { success = true, feedback = feedbackText });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
        }

        private string BuildCoachingPrompt(Story story, string element)
        {
            var rubrics = _context.StarRubrics.ToList();
            var rubric = rubrics.FirstOrDefault(r => r.Section == element);

            var prompt = $@"The candidate is preparing for a technical interview and has submitted a STAR story.

CANDIDATE'S STORY:
Situation: {story.Situation}
Task: {story.Task ?? "(Not provided)"}
Action: {story.Action ?? "(Not provided)"}
Result: {story.Result ?? "(Not provided)"}

COACHING GUIDANCE:
Section: {element}
Guidance: {rubric?.Guidance}
Details: {rubric?.GuidanceDetail}

Please provide:
1. A score (1-5) for this element
2. Specific feedback on what's strong
3. Single highest impact area to improve
4. A concrete suggestion for how to strengthen this part

Keep your response concise (under 200 words).";

            return prompt;
        }
    }
}