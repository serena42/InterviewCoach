using Microsoft.AspNetCore.Mvc;
using InterviewCoach.Models;

namespace InterviewCoach.Controllers
{
    public class QuestionsController : Controller
    {
        private readonly InterviewCoachContext _context;

        public QuestionsController(InterviewCoachContext context)
        {
            _context = context;
        }

        private bool IsLoggedIn()
        {
            var username = HttpContext.Session.GetString("Username");
            return !string.IsNullOrEmpty(username);
        }

        private void EnsureLoggedIn()
        {
            if (!IsLoggedIn())
            {
                throw new UnauthorizedAccessException("User must be logged in");
            }
        }

        public IActionResult Index()
        {
            EnsureLoggedIn();
            var questions = _context.Questions.ToList();
            return View(questions);
        }

        public IActionResult Develop(int id)
        {
            EnsureLoggedIn();
            var question = _context.Questions.FirstOrDefault(q => q.Id == id);
            if (question == null)
            {
                return NotFound();
            }
            return View(question);
        }

        public IActionResult Practice(int id)
        {
            EnsureLoggedIn();
            var question = _context.Questions.FirstOrDefault(q => q.Id == id);
            if (question == null)
            {
                return NotFound();
            }
            return View(question);
        }
    }
}