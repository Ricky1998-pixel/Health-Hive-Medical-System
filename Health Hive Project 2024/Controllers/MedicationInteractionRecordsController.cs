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
    public class MedicationInteractionRecordsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MedicationInteractionRecordsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: MedicationInteractionRecords
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.MedicationInteractionRecords.Include(m => m.Admin).Include(m => m.Medication);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: MedicationInteractionRecords/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medicationInteractionRecords = await _context.MedicationInteractionRecords
                .Include(m => m.Admin)
                .Include(m => m.Medication)
                .FirstOrDefaultAsync(m => m.InteractionID == id);
            if (medicationInteractionRecords == null)
            {
                return NotFound();
            }

            return View(medicationInteractionRecords);
        }

        // GET: MedicationInteractionRecords/Create
        public IActionResult Create()
        {
            ViewData["AdminID"] = new SelectList(_context.MedicalProfessionalRecords, "MedicalProfessionalID", "ContactNumber");
            ViewData["MedicationID"] = new SelectList(_context.MedicationRecords, "MedicationID", "MedicationName");
            return View();
        }

        // POST: MedicationInteractionRecords/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("InteractionID,MedicationID,InteractionType,Severity,Description,AdminID")] MedicationInteractionRecords medicationInteractionRecords)
        {
            if (ModelState.IsValid)
            {
                _context.Add(medicationInteractionRecords);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AdminID"] = new SelectList(_context.MedicalProfessionalRecords, "MedicalProfessionalID", "ContactNumber", medicationInteractionRecords.AdminID);
            ViewData["MedicationID"] = new SelectList(_context.MedicationRecords, "MedicationID", "MedicationName", medicationInteractionRecords.MedicationID);
            return View(medicationInteractionRecords);
        }

        // GET: MedicationInteractionRecords/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medicationInteractionRecords = await _context.MedicationInteractionRecords.FindAsync(id);
            if (medicationInteractionRecords == null)
            {
                return NotFound();
            }
            ViewData["AdminID"] = new SelectList(_context.MedicalProfessionalRecords, "MedicalProfessionalID", "ContactNumber", medicationInteractionRecords.AdminID);
            ViewData["MedicationID"] = new SelectList(_context.MedicationRecords, "MedicationID", "MedicationName", medicationInteractionRecords.MedicationID);
            return View(medicationInteractionRecords);
        }

        // POST: MedicationInteractionRecords/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("InteractionID,MedicationID,InteractionType,Severity,Description,AdminID")] MedicationInteractionRecords medicationInteractionRecords)
        {
            if (id != medicationInteractionRecords.InteractionID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(medicationInteractionRecords);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MedicationInteractionRecordsExists(medicationInteractionRecords.InteractionID))
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
            ViewData["AdminID"] = new SelectList(_context.MedicalProfessionalRecords, "MedicalProfessionalID", "ContactNumber", medicationInteractionRecords.AdminID);
            ViewData["MedicationID"] = new SelectList(_context.MedicationRecords, "MedicationID", "MedicationName", medicationInteractionRecords.MedicationID);
            return View(medicationInteractionRecords);
        }

        // GET: MedicationInteractionRecords/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medicationInteractionRecords = await _context.MedicationInteractionRecords
                .Include(m => m.Admin)
                .Include(m => m.Medication)
                .FirstOrDefaultAsync(m => m.InteractionID == id);
            if (medicationInteractionRecords == null)
            {
                return NotFound();
            }

            return View(medicationInteractionRecords);
        }

        // POST: MedicationInteractionRecords/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var medicationInteractionRecords = await _context.MedicationInteractionRecords.FindAsync(id);
            if (medicationInteractionRecords != null)
            {
                _context.MedicationInteractionRecords.Remove(medicationInteractionRecords);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MedicationInteractionRecordsExists(int id)
        {
            return _context.MedicationInteractionRecords.Any(e => e.InteractionID == id);
        }
    }
}
