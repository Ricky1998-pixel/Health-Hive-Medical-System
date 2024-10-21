using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Health_Hive_Project_2024.Data;

namespace Health_Hive_Project_2024.Models
{
    public class VitalsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VitalsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Vitals
        public async Task<IActionResult> Index()
        {
            return View(await _context.Vitals.ToListAsync());
        }

        // GET: Vitals/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vitals = await _context.Vitals
                .FirstOrDefaultAsync(m => m.VitalID == id);
            if (vitals == null)
            {
                return NotFound();
            }

            return View(vitals);
        }

        // GET: Vitals/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Vitals/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("VitalID,Name,VitalName,MinimumValue,MaxValue,CurrentValue,Date")] Vitals vitals)
        {
            if (ModelState.IsValid)
            {
                _context.Add(vitals);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(vitals);
        }

        // GET: Vitals/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vitals = await _context.Vitals.FindAsync(id);
            if (vitals == null)
            {
                return NotFound();
            }
            return View(vitals);
        }

        // POST: Vitals/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("VitalID,Name,VitalName,MinimumValue,MaxValue,CurrentValue,Date")] Vitals vitals)
        {
            if (id != vitals.VitalID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(vitals);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VitalsExists(vitals.VitalID))
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
            return View(vitals);
        }

        // GET: Vitals/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vitals = await _context.Vitals
                .FirstOrDefaultAsync(m => m.VitalID == id);
            if (vitals == null)
            {
                return NotFound();
            }

            return View(vitals);
        }

        // POST: Vitals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var vitals = await _context.Vitals.FindAsync(id);
            if (vitals != null)
            {
                _context.Vitals.Remove(vitals);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VitalsExists(int id)
        {
            return _context.Vitals.Any(e => e.VitalID == id);
        }
    }
}
