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
    public class FeedbackResponsesController : Controller
    {
        private readonly InterviewCoachContext _context;

        public FeedbackResponsesController(InterviewCoachContext context)
        {
            _context = context;
        }

        // GET: FeedbackResponses
        public async Task<IActionResult> Index()
        {
            return View(await _context.FeedbackResponse.ToListAsync());
        }

        // GET: FeedbackResponses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var feedbackResponse = await _context.FeedbackResponse
                .FirstOrDefaultAsync(m => m.Id == id);
            if (feedbackResponse == null)
            {
                return NotFound();
            }

            return View(feedbackResponse);
        }

        // GET: FeedbackResponses/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: FeedbackResponses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,SituationScore,TaskScore,ActionScore,ResultScore,SituationFeedback,TaskFeedback,ActionFeedback,ResultFeedback,OverallFeedback")] FeedbackResponse feedbackResponse)
        {
            if (ModelState.IsValid)
            {
                _context.Add(feedbackResponse);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(feedbackResponse);
        }

        // GET: FeedbackResponses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var feedbackResponse = await _context.FeedbackResponse.FindAsync(id);
            if (feedbackResponse == null)
            {
                return NotFound();
            }
            return View(feedbackResponse);
        }

        // POST: FeedbackResponses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,SituationScore,TaskScore,ActionScore,ResultScore,SituationFeedback,TaskFeedback,ActionFeedback,ResultFeedback,OverallFeedback")] FeedbackResponse feedbackResponse)
        {
            if (id != feedbackResponse.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(feedbackResponse);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FeedbackResponseExists(feedbackResponse.Id))
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
            return View(feedbackResponse);
        }

        // GET: FeedbackResponses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var feedbackResponse = await _context.FeedbackResponse
                .FirstOrDefaultAsync(m => m.Id == id);
            if (feedbackResponse == null)
            {
                return NotFound();
            }

            return View(feedbackResponse);
        }

        // POST: FeedbackResponses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var feedbackResponse = await _context.FeedbackResponse.FindAsync(id);
            if (feedbackResponse != null)
            {
                _context.FeedbackResponse.Remove(feedbackResponse);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FeedbackResponseExists(int id)
        {
            return _context.FeedbackResponse.Any(e => e.Id == id);
        }
    }
}
