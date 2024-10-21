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
    public class PatientMedicationsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PatientMedicationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: PatientMedications
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.PatientMedications.Include(p => p.Medication).Include(p => p.Patient);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: PatientMedications/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var patientMedication = await _context.PatientMedications
                .Include(p => p.Medication)
                .Include(p => p.Patient)
                .FirstOrDefaultAsync(m => m.PatientMedicationID == id);
            if (patientMedication == null)
            {
                return NotFound();
            }

            return View(patientMedication);
        }

        // GET: PatientMedications/Create
        public IActionResult Create()
        {
            ViewData["MedicationID"] = new SelectList(_context.MedicationRecords, "MedicationID", "MedicationName");
            ViewData["PatientID"] = new SelectList(_context.Patients, "PatientID", "Address");
            return View();
        }

        // POST: PatientMedications/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PatientMedicationID,PatientID,MedicationID")] PatientMedication patientMedication)
        {
            if (ModelState.IsValid)
            {
                _context.Add(patientMedication);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MedicationID"] = new SelectList(_context.MedicationRecords, "MedicationID", "MedicationName", patientMedication.MedicationID);
            ViewData["PatientID"] = new SelectList(_context.Patients, "PatientID", "Address", patientMedication.PatientID);
            return View(patientMedication);
        }

        // GET: PatientMedications/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var patientMedication = await _context.PatientMedications.FindAsync(id);
            if (patientMedication == null)
            {
                return NotFound();
            }
            ViewData["MedicationID"] = new SelectList(_context.MedicationRecords, "MedicationID", "MedicationName", patientMedication.MedicationID);
            ViewData["PatientID"] = new SelectList(_context.Patients, "PatientID", "Address", patientMedication.PatientID);
            return View(patientMedication);
        }

        // POST: PatientMedications/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PatientMedicationID,PatientID,MedicationID")] PatientMedication patientMedication)
        {
            if (id != patientMedication.PatientMedicationID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(patientMedication);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PatientMedicationExists(patientMedication.PatientMedicationID))
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
            ViewData["MedicationID"] = new SelectList(_context.MedicationRecords, "MedicationID", "MedicationName", patientMedication.MedicationID);
            ViewData["PatientID"] = new SelectList(_context.Patients, "PatientID", "Address", patientMedication.PatientID);
            return View(patientMedication);
        }

        // GET: PatientMedications/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var patientMedication = await _context.PatientMedications
                .Include(p => p.Medication)
                .Include(p => p.Patient)
                .FirstOrDefaultAsync(m => m.PatientMedicationID == id);
            if (patientMedication == null)
            {
                return NotFound();
            }

            return View(patientMedication);
        }

        // POST: PatientMedications/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var patientMedication = await _context.PatientMedications.FindAsync(id);
            if (patientMedication != null)
            {
                _context.PatientMedications.Remove(patientMedication);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PatientMedicationExists(int id)
        {
            return _context.PatientMedications.Any(e => e.PatientMedicationID == id);
        }
    }
}
