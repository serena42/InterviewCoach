using InterviewCoach.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InterviewCoach.Controllers
{
    public class AuthController : Controller
    {
        private readonly InterviewCoachContext _context;

        public AuthController(InterviewCoachContext context)
        {
            _context = context;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                ModelState.AddModelError("", "Username is required");
                return View();
            }

            // Ensure the user exists in the database
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
            if (user == null)
            {
                user = new User { Username = username, CreatedAt = DateTime.UtcNow };
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
            }

            // Store username in session
            HttpContext.Session.SetString("Username", username);
            return RedirectToAction("Dashboard", "Home");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}