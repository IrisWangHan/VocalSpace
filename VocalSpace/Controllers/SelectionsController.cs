using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VocalSpace.Models;

namespace VocalSpace.Controllers
{
    public class SelectionsController : Controller
    {
        private readonly VocalSpaceDbContext _context;

        public SelectionsController(VocalSpaceDbContext context)
        {
            _context = context;
        }

        // GET: Selections
        public async Task<IActionResult> Index()
        {
            return View(await _context.Selections.ToListAsync());
        }

        // GET: Selections/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var selection = await _context.Selections
                .FirstOrDefaultAsync(m => m.SelectionId == id);
            if (selection == null)
            {
                return NotFound();
            }

            return View(selection);
        }

        // GET: Selections/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Selections/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SelectionId,Title,SelectionCoverPath,CreateTime,StartDate,EndDate,VotingStartDate,VotingEndDate,Visible,Description,UserId")] Selection selection)
        {
            if (ModelState.IsValid)
            {
                _context.Add(selection);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(selection);
        }

        // GET: Selections/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var selection = await _context.Selections.FindAsync(id);
            if (selection == null)
            {
                return NotFound();
            }
            return View(selection);
        }

        // POST: Selections/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("SelectionId,Title,SelectionCoverPath,CreateTime,StartDate,EndDate,VotingStartDate,VotingEndDate,Visible,Description,UserId")] Selection selection)
        {
            if (id != selection.SelectionId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(selection);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SelectionExists(selection.SelectionId))
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
            return View(selection);
        }

        // GET: Selections/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var selection = await _context.Selections
                .FirstOrDefaultAsync(m => m.SelectionId == id);
            if (selection == null)
            {
                return NotFound();
            }

            return View(selection);
        }

        // POST: Selections/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var selection = await _context.Selections.FindAsync(id);
            if (selection != null)
            {
                _context.Selections.Remove(selection);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SelectionExists(long id)
        {
            return _context.Selections.Any(e => e.SelectionId == id);
        }
    }
}
