using Microsoft.AspNetCore.Mvc;

namespace InterviewCoach.Controllers
{
    public class AuthController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                ModelState.AddModelError("", "Username is required");
                return View();
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