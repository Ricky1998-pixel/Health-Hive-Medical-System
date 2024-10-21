using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Health_Hive_Project_2024.Data;
using Health_Hive_Project_2024.Models;

namespace Health_Hive_Project_2024.Controllers
{
    public class DosageFormsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DosageFormsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: DosageForms
        public async Task<IActionResult> Index()
        {
            return View(await _context.DosageForm.ToListAsync());
        }

        // GET: DosageForms/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dosageForm = await _context.DosageForm
                .FirstOrDefaultAsync(m => m.DosageFormID == id);
            if (dosageForm == null)
            {
                return NotFound();
            }

            return View(dosageForm);
        }

        // GET: DosageForms/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: DosageForms/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DosageFormID,Form")] DosageForm dosageForm)
        {
            if (ModelState.IsValid)
            {
                _context.Add(dosageForm);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(dosageForm);
        }

        // GET: DosageForms/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dosageForm = await _context.DosageForm.FindAsync(id);
            if (dosageForm == null)
            {
                return NotFound();
            }
            return View(dosageForm);
        }

        // POST: DosageForms/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DosageFormID,Form")] DosageForm dosageForm)
        {
            if (id != dosageForm.DosageFormID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dosageForm);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DosageFormExists(dosageForm.DosageFormID))
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
            return View(dosageForm);
        }

        // GET: DosageForms/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dosageForm = await _context.DosageForm
                .FirstOrDefaultAsync(m => m.DosageFormID == id);
            if (dosageForm == null)
            {
                return NotFound();
            }

            return View(dosageForm);
        }

        // POST: DosageForms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var dosageForm = await _context.DosageForm.FindAsync(id);
            if (dosageForm != null)
            {
                _context.DosageForm.Remove(dosageForm);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DosageFormExists(int id)
        {
            return _context.DosageForm.Any(e => e.DosageFormID == id);
        }
    }
}
