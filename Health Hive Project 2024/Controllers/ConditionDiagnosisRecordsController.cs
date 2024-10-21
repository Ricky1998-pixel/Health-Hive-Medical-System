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
    public class ConditionDiagnosisRecordsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ConditionDiagnosisRecordsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ConditionDiagnosisRecords
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.ConditionDiagnosisRecords.Include(c => c.Patient);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: ConditionDiagnosisRecords/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var conditionDiagnosisRecords = await _context.ConditionDiagnosisRecords
                .Include(c => c.Patient)
                .FirstOrDefaultAsync(m => m.DiagnosisID == id);
            if (conditionDiagnosisRecords == null)
            {
                return NotFound();
            }

            return View(conditionDiagnosisRecords);
        }

        // GET: ConditionDiagnosisRecords/Create
        public IActionResult Create()
        {
            ViewData["AdminID"] = new SelectList(_context.MedicalProfessionalRecords, "MedicalProfessionalID", "ContactNumber");
            ViewData["PatientID"] = new SelectList(_context.Patients, "PatientID", "Address");
            return View();
        }

        // POST: ConditionDiagnosisRecords/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DiagnosisID,PatientID,DiagnosisCode,DiagnosisName,Symptoms,Treatment,AdminID")] ConditionDiagnosisRecords conditionDiagnosisRecords)
        {
            if (ModelState.IsValid)
            {
                _context.Add(conditionDiagnosisRecords);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PatientID"] = new SelectList(_context.Patients, "PatientID", "Address", conditionDiagnosisRecords.PatientID);
            return View(conditionDiagnosisRecords);
        }

        // GET: ConditionDiagnosisRecords/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var conditionDiagnosisRecords = await _context.ConditionDiagnosisRecords.FindAsync(id);
            if (conditionDiagnosisRecords == null)
            {
                return NotFound();
            }
            ViewData["PatientID"] = new SelectList(_context.Patients, "PatientID", "Name", conditionDiagnosisRecords.PatientID);
            return View(conditionDiagnosisRecords);
        }

        // POST: ConditionDiagnosisRecords/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DiagnosisID,PatientID,DiagnosisCode,DiagnosisName,Symptoms,Treatment,AdminID")] ConditionDiagnosisRecords conditionDiagnosisRecords)
        {
            if (id != conditionDiagnosisRecords.DiagnosisID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(conditionDiagnosisRecords);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ConditionDiagnosisRecordsExists(conditionDiagnosisRecords.DiagnosisID))
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
            ViewData["PatientID"] = new SelectList(_context.Patients, "PatientID", "Address", conditionDiagnosisRecords.PatientID);
            return View(conditionDiagnosisRecords);
        }

        // GET: ConditionDiagnosisRecords/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var conditionDiagnosisRecords = await _context.ConditionDiagnosisRecords
                .Include(c => c.Patient)
                .FirstOrDefaultAsync(m => m.DiagnosisID == id);
            if (conditionDiagnosisRecords == null)
            {
                return NotFound();
            }

            return View(conditionDiagnosisRecords);
        }

        // POST: ConditionDiagnosisRecords/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var conditionDiagnosisRecords = await _context.ConditionDiagnosisRecords.FindAsync(id);
            if (conditionDiagnosisRecords != null)
            {
                _context.ConditionDiagnosisRecords.Remove(conditionDiagnosisRecords);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ConditionDiagnosisRecordsExists(int id)
        {
            return _context.ConditionDiagnosisRecords.Any(e => e.DiagnosisID == id);
        }
    }
}
