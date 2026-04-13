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
    public class StarRubricsController : Controller
    {
        private readonly InterviewCoachContext _context;

        public StarRubricsController(InterviewCoachContext context)
        {
            _context = context;
        }

        // GET: StarRubrics
        public async Task<IActionResult> Index()
        {
            return View(await _context.StarRubrics.ToListAsync());
        }

        // GET: StarRubrics/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var starRubric = await _context.StarRubrics
                .FirstOrDefaultAsync(m => m.Id == id);
            if (starRubric == null)
            {
                return NotFound();
            }

            return View(starRubric);
        }

        // GET: StarRubrics/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: StarRubrics/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Section,Guidance,GuidanceDetail")] StarRubric starRubric)
        {
            if (ModelState.IsValid)
            {
                _context.Add(starRubric);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(starRubric);
        }

        // GET: StarRubrics/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var starRubric = await _context.StarRubrics.FindAsync(id);
            if (starRubric == null)
            {
                return NotFound();
            }
            return View(starRubric);
        }

        // POST: StarRubrics/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Section,Guidance,GuidanceDetail")] StarRubric starRubric)
        {
            if (id != starRubric.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(starRubric);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StarRubricExists(starRubric.Id))
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
            return View(starRubric);
        }

        // GET: StarRubrics/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var starRubric = await _context.StarRubrics
                .FirstOrDefaultAsync(m => m.Id == id);
            if (starRubric == null)
            {
                return NotFound();
            }

            return View(starRubric);
        }

        // POST: StarRubrics/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var starRubric = await _context.StarRubrics.FindAsync(id);
            if (starRubric != null)
            {
                _context.StarRubrics.Remove(starRubric);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StarRubricExists(int id)
        {
            return _context.StarRubrics.Any(e => e.Id == id);
        }
    }
}
