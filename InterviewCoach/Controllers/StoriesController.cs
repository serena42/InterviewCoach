using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using InterviewCoach.Models;

namespace InterviewCoach.Controllers
{
    public class StoriesController : Controller
    {
        private readonly InterviewCoachContext _context;

        public StoriesController(InterviewCoachContext context)
        {
            _context = context;
        }

        // GET: Stories
        public async Task<IActionResult> Index()
        {
            var username = HttpContext.Session.GetString("Username");
            if (string.IsNullOrEmpty(username))
            {
                return RedirectToAction("Login", "Auth");
            }

            var user = await _context.Users
                .Include(u => u.Stories)
                    .ThenInclude(s => s.Question)
                .FirstOrDefaultAsync(u => u.Username == username);

            if (user == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            var allQuestions = await _context.Questions.AsNoTracking().ToListAsync();
            var userStories = user.Stories.Where(s => s.QuestionId.HasValue).ToDictionary(s => s.QuestionId!.Value);

            // Create a list of stories. If a user doesn't have one for a question, show a placeholder.
            var model = allQuestions.Select(q => {
                if (userStories.TryGetValue(q.Id, out var existingStory))
                {
                    return existingStory;
                }
                return new Story 
                { 
                    Id = 0,
                    QuestionId = q.Id,
                    Question = q,
                    Situation = q.QuestionText, // Use question text as the placeholder for Situation
                    AuthorName = username,
                    CreatedAt = DateTime.MinValue // Flag as placeholder
                };
            }).ToList();

            // Add custom stories (those without a QuestionId)
            var customStories = user.Stories.Where(s => !s.QuestionId.HasValue).ToList();
            model.AddRange(customStories);

            return View(model);
        }

        private async Task<Story?> GetOrCreateStoryAsync(int? id, int? questionId)
        {
            var username = HttpContext.Session.GetString("Username");
            if (string.IsNullOrEmpty(username)) return null;

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
            if (user == null) return null;

            Story? story = null;

            if (id.HasValue && id > 0)
            {
                story = await _context.Stories
                    .Include(s => s.Question)
                    .FirstOrDefaultAsync(s => s.Id == id && s.UserId == user.Id);
            }
            else if (questionId.HasValue)
            {
                story = await _context.Stories
                    .Include(s => s.Question)
                    .FirstOrDefaultAsync(s => s.QuestionId == questionId && s.UserId == user.Id);

                if (story == null)
                {
                    var question = await _context.Questions.FindAsync(questionId.Value);
                    if (question != null)
                    {
                        story = new Story
                        {
                            QuestionId = questionId,
                            Question = question,
                            UserId = user.Id,
                            AuthorName = username,
                            Situation = question.QuestionText,
                            CreatedAt = DateTime.UtcNow
                        };
                        _context.Stories.Add(story);
                        await _context.SaveChangesAsync();
                    }
                }
            }

            return story;
        }

        // GET: Stories/Details/5?questionId=1
        public async Task<IActionResult> Details(int? id, int? questionId)
        {
            var story = await GetOrCreateStoryAsync(id, questionId);
            if (story == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            return View(story);
        }

        // GET: Stories/Create
        public IActionResult Create(int? questionId)
        {
            if (questionId.HasValue)
            {
                var question = _context.Questions.Find(questionId.Value);
                if (question != null)
                {
                    return View(new Story { QuestionId = questionId, Situation = question.QuestionText });
                }
            }
            return View();
        }

        // POST: Stories/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,QuestionId,Situation,Task,Action,Result,AuthorName")] Story story)
        {
            var username = HttpContext.Session.GetString("Username");
            if (string.IsNullOrEmpty(username)) return RedirectToAction("Login", "Auth");

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
            if (user == null) return RedirectToAction("Login", "Auth");

            if (ModelState.IsValid)
            {
                story.UserId = user.Id;
                story.CreatedAt = DateTime.UtcNow;
                _context.Add(story);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(story);
        }

        // GET: Stories/Edit/5?questionId=1
        public async Task<IActionResult> Edit(int? id, int? questionId)
        {
            var story = await GetOrCreateStoryAsync(id, questionId);
            if (story == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            return View(story);
        }

        // POST: Stories/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,QuestionId,UserId,Situation,Task,Action,Result,SituationScore,TaskScore,ActionScore,ResultScore,AuthorName,CreatedAt,CoachingFeedback")] Story story)
        {
            if (id != story.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    story.CreatedAt = DateTime.UtcNow; // Update timestamp
                    _context.Update(story);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StoryExists(story.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(story);
        }

        // GET: Stories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var story = await _context.Stories
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);

            if (story == null) return NotFound();

            return View(story);
        }

        // POST: Stories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var story = await _context.Stories.FindAsync(id);
            if (story != null)
            {
                _context.Stories.Remove(story);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StoryExists(int id)
        {
            return _context.Stories.Any(e => e.Id == id);
        }
    }
}
