using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MatchBetting.Data;
using MatchBetting.Models;
using System.Security.Claims;

namespace MatchBetting.Controllers
{
    public class SideBetsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SideBetsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: SideBets
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.SideBettings.Include(s => s.User);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: SideBets/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sideBet = await _context.SideBettings
                .Include(s => s.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sideBet == null)
            {
                return NotFound();
            }

            return View(sideBet);
        }

        // GET: SideBets/Create
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "Id");
            return View();
        }

        // POST: SideBets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Toppscorer,MostCards,WinnerTeam,UserId")] SideBet sideBet)
        {
            if (ModelState.IsValid)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                sideBet.UserId = userId;
                _context.Add(sideBet);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }


            ViewData["UserId"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "Id", sideBet.UserId);
            return View(sideBet);
        }

        // GET: SideBets/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sideBet = await _context.SideBettings.FindAsync(id);
            if (sideBet == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "Id", sideBet.UserId);
            return View(sideBet);
        }

        // POST: SideBets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Toppscorer,MostCards,WinnerTeam,UserId")] SideBet sideBet)
        {
            if (id != sideBet.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(sideBet);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SideBetExists(sideBet.Id))
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
            ViewData["UserId"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "Id", sideBet.UserId);
            return View(sideBet);
        }

        // GET: SideBets/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sideBet = await _context.SideBettings
                .Include(s => s.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sideBet == null)
            {
                return NotFound();
            }

            return View(sideBet);
        }

        // POST: SideBets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var sideBet = await _context.SideBettings.FindAsync(id);
            if (sideBet != null)
            {
                _context.SideBettings.Remove(sideBet);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SideBetExists(int id)
        {
            return _context.SideBettings.Any(e => e.Id == id);
        }
    }
}
