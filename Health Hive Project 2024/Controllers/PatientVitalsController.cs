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
    public class PatientVitalsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PatientVitalsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetPatientVitals(int id)
        {
            var patientVitals = _context.PatientVitals
                .Include(pv => pv.Vitals)
                .Where(v => v.PatientID == id)
                .ToList();

            if (patientVitals == null || !patientVitals.Any())
            {
                return PartialView("_PatientVitals", new List<PatientVitals>());
            }

            return PartialView("_PatientVitals", patientVitals);
        }




        // GET: PatientVitals
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.PatientVitals.Include(p => p.Patient);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: PatientVitals/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var patientVitals = await _context.PatientVitals
                .Include(p => p.Patient)
                .FirstOrDefaultAsync(m => m.PatientVitalID == id);
            if (patientVitals == null)
            {
                return NotFound();
            }

            return View(patientVitals);
        }

        // GET: PatientVitals/Create
        public IActionResult Create()
        {
            ViewData["PatientID"] = new SelectList(_context.Patients, "PatientID", "Address");
            return View();
        }

        // POST: PatientVitals/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PatientVitalID,PatientID,HeartRate,SystolicBloodPressure,DiastolicBloodPressure,RespirationRate,BodyTemperature,OxygenSaturation,BloodGlucoseLevel,RecordedDateTime,Notes")] PatientVitals patientVitals)
        {
            
                _context.Add(patientVitals);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            
            ViewData["PatientID"] = new SelectList(_context.Patients, "PatientID", "Address", patientVitals.PatientID);
            return View(patientVitals);
        }

        // GET: PatientVitals/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var patientVitals = await _context.PatientVitals.FindAsync(id);
            if (patientVitals == null)
            {
                return NotFound();
            }
            ViewData["PatientID"] = new SelectList(_context.Patients, "PatientID", "Address", patientVitals.PatientID);
            return View(patientVitals);
        }

        // POST: PatientVitals/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PatientVitalID,PatientID,HeartRate,SystolicBloodPressure,DiastolicBloodPressure,RespirationRate,BodyTemperature,OxygenSaturation,BloodGlucoseLevel,RecordedDateTime,Notes")] PatientVitals patientVitals)
        {
            if (id != patientVitals.PatientVitalID)
            {
                return NotFound();
            }

            
                try
                {
                    _context.Update(patientVitals);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PatientVitalsExists(patientVitals.PatientVitalID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            
            ViewData["PatientID"] = new SelectList(_context.Patients, "PatientID", "Address", patientVitals.PatientID);
            return View(patientVitals);
        }

        // GET: PatientVitals/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var patientVitals = await _context.PatientVitals
                .Include(p => p.Patient)
                .FirstOrDefaultAsync(m => m.PatientVitalID == id);
            if (patientVitals == null)
            {
                return NotFound();
            }

            return View(patientVitals);
        }

        // POST: PatientVitals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var patientVitals = await _context.PatientVitals.FindAsync(id);
            if (patientVitals != null)
            {
                _context.PatientVitals.Remove(patientVitals);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PatientVitalsExists(int id)
        {
            return _context.PatientVitals.Any(e => e.PatientVitalID == id);
        }
    }
}
