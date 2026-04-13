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
    public class PracticeAttemptsController : Controller
    {
        private readonly InterviewCoachContext _context;

        public PracticeAttemptsController(InterviewCoachContext context)
        {
            _context = context;
        }

        // GET: PracticeAttempts
        public async Task<IActionResult> Index()
        {
            var interviewCoachContext = _context.PracticeAttempts.Include(p => p.Story);
            return View(await interviewCoachContext.ToListAsync());
        }

        // GET: PracticeAttempts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var practiceAttempt = await _context.PracticeAttempts
                .Include(p => p.Story)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (practiceAttempt == null)
            {
                return NotFound();
            }

            return View(practiceAttempt);
        }

        // GET: PracticeAttempts/Create
        public IActionResult Create()
        {
            ViewData["StoryId"] = new SelectList(_context.Stories, "Id", "Id");
            return View();
        }

        // POST: PracticeAttempts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,StoryId,AttemptDate,AttemptScore,Notes")] PracticeAttempt practiceAttempt)
        {
            if (ModelState.IsValid)
            {
                _context.Add(practiceAttempt);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["StoryId"] = new SelectList(_context.Stories, "Id", "Id", practiceAttempt.StoryId);
            return View(practiceAttempt);
        }

        // GET: PracticeAttempts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var practiceAttempt = await _context.PracticeAttempts.FindAsync(id);
            if (practiceAttempt == null)
            {
                return NotFound();
            }
            ViewData["StoryId"] = new SelectList(_context.Stories, "Id", "Id", practiceAttempt.StoryId);
            return View(practiceAttempt);
        }

        // POST: PracticeAttempts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,StoryId,AttemptDate,AttemptScore,Notes")] PracticeAttempt practiceAttempt)
        {
            if (id != practiceAttempt.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(practiceAttempt);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PracticeAttemptExists(practiceAttempt.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["StoryId"] = new SelectList(_context.Stories, "Id", "Id", practiceAttempt.StoryId);
            return View(practiceAttempt);
        }

        // GET: PracticeAttempts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var practiceAttempt = await _context.PracticeAttempts
                .Include(p => p.Story)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (practiceAttempt == null)
            {
                return NotFound();
            }

            return View(practiceAttempt);
        }

        // POST: PracticeAttempts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var practiceAttempt = await _context.PracticeAttempts.FindAsync(id);
            if (practiceAttempt != null)
            {
                _context.PracticeAttempts.Remove(practiceAttempt);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PracticeAttemptExists(int id)
        {
            return _context.PracticeAttempts.Any(e => e.Id == id);
        }
    }
}
