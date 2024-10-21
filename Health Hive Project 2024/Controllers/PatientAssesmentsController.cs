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
    public class PatientAssesmentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PatientAssesmentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: PatientAssesments
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.PatientAssesments.Include(p => p.Anaesthesiologist).Include(p => p.Patient);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: PatientAssesments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var patientAssesment = await _context.PatientAssesments
                .Include(p => p.Anaesthesiologist)
                .Include(p => p.Patient)
                .FirstOrDefaultAsync(m => m.PatientAssesmentID == id);
            if (patientAssesment == null)
            {
                return NotFound();
            }

            return View(patientAssesment);
        }

        // GET: PatientAssesments/Create
        public IActionResult Create()
        {
            ViewData["AnaesthesiologistID"] = new SelectList(_context.MedicalProfessionalRecords, "MedicalProfessionalID", "ContactNumber");
            ViewData["PatientID"] = new SelectList(_context.Patients, "PatientID", "Address");
            return View();
        }

        // POST: PatientAssesments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PatientAssesmentID,AnaesthesiologistID,PatientID,AssasmentDate")] PatientAssesment patientAssesment)
        {
            if (ModelState.IsValid)
            {
                _context.Add(patientAssesment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AnaesthesiologistID"] = new SelectList(_context.MedicalProfessionalRecords, "MedicalProfessionalID", "ContactNumber", patientAssesment.Id);
            ViewData["PatientID"] = new SelectList(_context.Patients, "PatientID", "Address", patientAssesment.PatientID);
            return View(patientAssesment);
        }

        // GET: PatientAssesments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var patientAssesment = await _context.PatientAssesments.FindAsync(id);
            if (patientAssesment == null)
            {
                return NotFound();
            }
            ViewData["Id"] = new SelectList(_context.MedicalProfessionalRecords, "MedicalProfessionalID", "ContactNumber", patientAssesment.Id);
            ViewData["PatientID"] = new SelectList(_context.Patients, "PatientID", "Address", patientAssesment.PatientID);
            return View(patientAssesment);
        }

        // POST: PatientAssesments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PatientAssesmentID,Id,PatientID,AssasmentDate")] PatientAssesment patientAssesment)
        {
            if (id != patientAssesment.PatientAssesmentID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(patientAssesment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PatientAssesmentExists(patientAssesment.PatientAssesmentID))
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
            ViewData["Id"] = new SelectList(_context.MedicalProfessionalRecords, "MedicalProfessionalID", "ContactNumber", patientAssesment.Id);
            ViewData["PatientID"] = new SelectList(_context.Patients, "PatientID", "Address", patientAssesment.PatientID);
            return View(patientAssesment);
        }

        // GET: PatientAssesments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var patientAssesment = await _context.PatientAssesments
                .Include(p => p.Anaesthesiologist)
                .Include(p => p.Patient)
                .FirstOrDefaultAsync(m => m.PatientAssesmentID == id);
            if (patientAssesment == null)
            {
                return NotFound();
            }

            return View(patientAssesment);
        }

        // POST: PatientAssesments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var patientAssesment = await _context.PatientAssesments.FindAsync(id);
            if (patientAssesment != null)
            {
                _context.PatientAssesments.Remove(patientAssesment);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PatientAssesmentExists(int id)
        {
            return _context.PatientAssesments.Any(e => e.PatientAssesmentID == id);
        }
    }
}
