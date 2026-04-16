using Microsoft.AspNetCore.Mvc;
using InterviewCoach.Models;
using Mscc.GenerativeAI;
using Microsoft.EntityFrameworkCore;
using Mscc.GenerativeAI.Types;

namespace InterviewCoach.Controllers
{
    public class CoachController : Controller
    {
        private readonly InterviewCoachContext _context;
        private readonly GenerativeModel _model;

        public CoachController(InterviewCoachContext context)
        {
            _context = context;
            // Initialize Gemini client with your API key from environment
            // The SDK automatically reads GEMINI_API_KEY from environment variables
            System.Console.WriteLine("[DEBUG] CoachController constructor: Initializing GoogleAI...");
            var googleAI = new GoogleAI();
            _model = googleAI.GenerativeModel(model: Model.Gemini25Flash);
            System.Console.WriteLine("[DEBUG] GenerativeModel created successfully");
        }

        // GET: Coach/Test - Quick test endpoint
        [HttpGet("test")]
        public async Task<IActionResult> TestGeminiConnection()
        {
            try
            {
                System.Console.WriteLine("[TEST] Starting Gemini connection test...");
                var response = await _model.GenerateContent("Say 'Gemini is working!' in exactly those words.");
                System.Console.WriteLine("[TEST] Got response from Gemini");

                return Json(new
                {
                    success = true,
                    message = response.Text
                });
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"[TEST ERROR] {ex.Message}");
                return Json(new { success = false, error = ex.Message });
            }
        }

        // POST: Coach/GetFeedback
        [HttpPost]
        public async Task<IActionResult> GetFeedback(int storyId, string element)
        {
            try
            {
                System.Console.WriteLine($"[DEBUG] GetFeedback called with storyId={storyId}, element={element}");

                // First, let's see what stories exist in the database
                var allStories = _context.Stories.ToList();
                System.Console.WriteLine($"[DEBUG] Total stories in database: {allStories.Count}");
                foreach (var s in allStories)
                {
                    System.Console.WriteLine($"[DEBUG]   Story ID={s.Id}, UserId={s.UserId}, Situation={s.Situation?.Substring(0, Math.Min(50, s.Situation?.Length ?? 0))}");
                }

                // Now look for the specific story
                var story = await _context.Stories.FindAsync(storyId);
                if (story == null)
                {
                    System.Console.WriteLine($"[ERROR] Story {storyId} not found in database");
                    return Json(new { success = false, error = $"Story {storyId} not found" });
                }
                System.Console.WriteLine($"[DEBUG] Found story: {story.Situation}");

                // Build the prompt for Gemini
                var prompt = BuildCoachingPrompt(story, element);
                System.Console.WriteLine($"[DEBUG] Built prompt, length: {prompt.Length}");

                // Call Gemini API
                var systemPrompt = "You are an expert interview coach helping candidates improve their STAR stories for technical interviews.";
                var fullPrompt = systemPrompt + "\n\n" + prompt;

                System.Console.WriteLine("[DEBUG] Calling Gemini API...");
                var response = await _model.GenerateContent(fullPrompt);
                System.Console.WriteLine("[DEBUG] Gemini API returned successfully");

                var feedbackText = response.Text;
                if (string.IsNullOrEmpty(feedbackText))
                {
                    System.Console.WriteLine("[ERROR] Gemini returned empty response");
                    return Json(new { success = false, error = "No response from Gemini API" });
                }

                System.Console.WriteLine($"[DEBUG] Got feedback, length: {feedbackText.Length}");

                // Extract the score from the feedback (looking for "1-5" or "score" pattern)
                int? score = ExtractScore(feedbackText);
                System.Console.WriteLine($"[DEBUG] Extracted score: {score}");

                // Save feedback to the story
                story.CoachingFeedback = feedbackText;

                // Save the score to the appropriate field based on element
                if (score.HasValue)
                {
                    switch (element.ToLower())
                    {
                        case "situation":
                            story.SituationScore = score.Value;
                            break;
                        case "task":
                            story.TaskScore = score.Value;
                            break;
                        case "action":
                            story.ActionScore = score.Value;
                            break;
                        case "result":
                            story.ResultScore = score.Value;
                            break;
                    }
                }

                _context.Update(story);
                await _context.SaveChangesAsync();
                System.Console.WriteLine("[DEBUG] Saved feedback and score to database");

                return Json(new { success = true, feedback = feedbackText });
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"[ERROR] Exception: {ex.GetType().Name}: {ex.Message}");
                System.Console.WriteLine($"[ERROR] Stack trace: {ex.StackTrace}");
                return Json(new { success = false, error = ex.Message });
            }
        }

        private int? ExtractScore(string text)
        {
            // Look for patterns like "4/5" or "score: 4" or "score (4/5)"

            // First try to find X/5 pattern (e.g., "4/5")
            var lines = text.Split('\n');
            foreach (var line in lines)
            {
                // Look for pattern like "4/5"
                for (int i = 1; i <= 5; i++)
                {
                    string pattern = i + "/5";
                    if (line.Contains(pattern))
                    {
                        System.Console.WriteLine($"[DEBUG] Found score pattern '{pattern}'");
                        return i;
                    }
                }
            }

            // If no X/5 pattern found, look for "score:" followed by a number
            foreach (var line in lines)
            {
                if (line.Contains("score", System.StringComparison.OrdinalIgnoreCase))
                {
                    // Extract number after "score"
                    var words = line.Split(new[] { ' ', ':', '-' }, System.StringSplitOptions.RemoveEmptyEntries);
                    foreach (var word in words)
                    {
                        if (int.TryParse(word, out int num) && num >= 1 && num <= 5)
                        {
                            System.Console.WriteLine($"[DEBUG] Found score from 'score:' pattern: {num}");
                            return num;
                        }
                    }
                }
            }

            System.Console.WriteLine($"[DEBUG] No score pattern found in feedback");
            return null;
        }

        private string BuildCoachingPrompt(Story story, string element)
        {
            // Get the rubric from the database
            StarRubric? rubric = null;
            foreach (var r in _context.StarRubrics)
            {
                if (r.Section == element)
                {
                    rubric = r;
                    break;
                }
            }

            // Build the prompt string
            string guidance = rubric?.Guidance ?? "No guidance available";
            string guidanceDetail = rubric?.GuidanceDetail ?? "No details available";

            var prompt = $@"The candidate is preparing for a technical interview and has submitted a STAR story.

CANDIDATE'S STORY:
Situation: {story.Situation}
Task: {story.Task ?? "(Not provided)"}
Action: {story.Action ?? "(Not provided)"}
Result: {story.Result ?? "(Not provided)"}

COACHING GUIDANCE:
Section: {element}
Guidance: {guidance}
Details: {guidanceDetail}

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