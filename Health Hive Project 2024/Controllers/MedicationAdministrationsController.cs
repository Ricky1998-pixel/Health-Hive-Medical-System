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
    public class MedicationAdministrationsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MedicationAdministrationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: MedicationAdministrations
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.MedicationAdministrations.Include(m => m.Medication).Include(m => m.Nurse).Include(m => m.Patient);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: MedicationAdministrations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medicationAdministration = await _context.MedicationAdministrations
                .Include(m => m.Medication)
                .Include(m => m.Nurse)
                .Include(m => m.Patient)
                .FirstOrDefaultAsync(m => m.AdministrationID == id);
            if (medicationAdministration == null)
            {
                return NotFound();
            }

            return View(medicationAdministration);
        }

        // GET: MedicationAdministrations/Create
        public IActionResult Create()
        {
            ViewData["MedicationID"] = new SelectList(_context.MedicationRecords, "MedicationID", "MedicationName");
            ViewData["Id"] = new SelectList(_context.MedicalProfessionalRecords, "MedicalProfessionalID", "ContactNumber");
            ViewData["PatientID"] = new SelectList(_context.Patients, "PatientID", "Address");
            return View();
        }

        // POST: MedicationAdministrations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AdministrationID,MedicationID,Id,PatientID")] MedicationAdministration medicationAdministration)
        {
            if (ModelState.IsValid)
            {
                _context.Add(medicationAdministration);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MedicationID"] = new SelectList(_context.MedicationRecords, "MedicationID", "MedicationName", medicationAdministration.MedicationID);
            ViewData["Id"] = new SelectList(_context.MedicalProfessionalRecords, "MedicalProfessionalID", "ContactNumber", medicationAdministration.Id);
            ViewData["PatientID"] = new SelectList(_context.Patients, "PatientID", "Address", medicationAdministration.PatientID);
            return View(medicationAdministration);
        }

        // GET: MedicationAdministrations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medicationAdministration = await _context.MedicationAdministrations.FindAsync(id);
            if (medicationAdministration == null)
            {
                return NotFound();
            }
            ViewData["MedicationID"] = new SelectList(_context.MedicationRecords, "MedicationID", "MedicationName", medicationAdministration.MedicationID);
            ViewData["Id"] = new SelectList(_context.MedicalProfessionalRecords, "MedicalProfessionalID", "ContactNumber", medicationAdministration.Id);
            ViewData["PatientID"] = new SelectList(_context.Patients, "PatientID", "Address", medicationAdministration.PatientID);
            return View(medicationAdministration);
        }

        // POST: MedicationAdministrations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AdministrationID,MedicationID,Id,PatientID")] MedicationAdministration medicationAdministration)
        {
            if (id != medicationAdministration.AdministrationID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(medicationAdministration);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MedicationAdministrationExists(medicationAdministration.AdministrationID))
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
            ViewData["MedicationID"] = new SelectList(_context.MedicationRecords, "MedicationID", "MedicationName", medicationAdministration.MedicationID);
            ViewData["Id"] = new SelectList(_context.MedicalProfessionalRecords, "MedicalProfessionalID", "ContactNumber", medicationAdministration.Id);
            ViewData["PatientID"] = new SelectList(_context.Patients, "PatientID", "Address", medicationAdministration.PatientID);
            return View(medicationAdministration);
        }

        // GET: MedicationAdministrations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medicationAdministration = await _context.MedicationAdministrations
                .Include(m => m.Medication)
                .Include(m => m.Nurse)
                .Include(m => m.Patient)
                .FirstOrDefaultAsync(m => m.AdministrationID == id);
            if (medicationAdministration == null)
            {
                return NotFound();
            }

            return View(medicationAdministration);
        }

        // POST: MedicationAdministrations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var medicationAdministration = await _context.MedicationAdministrations.FindAsync(id);
            if (medicationAdministration != null)
            {
                _context.MedicationAdministrations.Remove(medicationAdministration);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MedicationAdministrationExists(int id)
        {
            return _context.MedicationAdministrations.Any(e => e.AdministrationID == id);
        }
    }
}
