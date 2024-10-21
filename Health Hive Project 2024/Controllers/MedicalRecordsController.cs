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
    public class MedicalRecordsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MedicalRecordsController(ApplicationDbContext context)
        {
            _context = context;
        }






        // GET: MedicalRecords
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.MedicalRecords.Include(m => m.Allergies);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: MedicalRecords/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medicalRecords = await _context.MedicalRecords
                .Include(m => m.Allergies)
                .FirstOrDefaultAsync(m => m.RecordsID == id);
            if (medicalRecords == null)
            {
                return NotFound();
            }

            return View(medicalRecords);
        }

        // GET: MedicalRecords/Create
        public IActionResult Create()
        {
            ViewData["AllergyID"] = new SelectList(_context.Allergies, "AllergyID", "AllergyDescription");
            return View();
        }

        // POST: MedicalRecords/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RecordsID,AllergyID,CurrentMedication")] MedicalRecords medicalRecords)
        {
            if (ModelState.IsValid)
            {
                _context.Add(medicalRecords);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AllergyID"] = new SelectList(_context.Allergies, "AllergyID", "AllergyDescription", medicalRecords.AllergyID);
            return View(medicalRecords);
        }

        // GET: MedicalRecords/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medicalRecords = await _context.MedicalRecords.FindAsync(id);
            if (medicalRecords == null)
            {
                return NotFound();
            }
            ViewData["AllergyID"] = new SelectList(_context.Allergies, "AllergyID", "AllergyDescription", medicalRecords.AllergyID);
            return View(medicalRecords);
        }

        // POST: MedicalRecords/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RecordsID,AllergyID,CurrentMedication")] MedicalRecords medicalRecords)
        {
            if (id != medicalRecords.RecordsID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(medicalRecords);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MedicalRecordsExists(medicalRecords.RecordsID))
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
            ViewData["AllergyID"] = new SelectList(_context.Allergies, "AllergyID", "AllergyDescription", medicalRecords.AllergyID);
            return View(medicalRecords);
        }

        // GET: MedicalRecords/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medicalRecords = await _context.MedicalRecords
                .Include(m => m.Allergies)
                .FirstOrDefaultAsync(m => m.RecordsID == id);
            if (medicalRecords == null)
            {
                return NotFound();
            }

            return View(medicalRecords);
        }

        // POST: MedicalRecords/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var medicalRecords = await _context.MedicalRecords.FindAsync(id);
            if (medicalRecords != null)
            {
                _context.MedicalRecords.Remove(medicalRecords);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MedicalRecordsExists(int id)
        {
            return _context.MedicalRecords.Any(e => e.RecordsID == id);
        }
    }
}
