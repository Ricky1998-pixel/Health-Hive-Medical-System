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
    public class DispensedPrescriptionsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DispensedPrescriptionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        //[HttpGet("DispensedPrescriptions/Report")]
        //public IActionResult Report(DateTime? startDate, DateTime? endDate)
        //{
        //    // Default to current month if no dates are provided
        //    if (startDate == null)
        //        startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        //    if (endDate == null)
        //        endDate = DateTime.Now;
        //    // Adjust endDate to cover the entire day
        //    endDate = endDate.Value.Date.AddDays(1).AddTicks(-1);

        //    // Fetch the logged-in pharmacist's identity (using email)
        //    var userName = User.Identity.Name;
        //    // Find the pharmacist in the database
        //    var pharmacist = _context.MedicalProfessionalRecords
        //                             .FirstOrDefault(m => m.EmailAddress == userName);
        //    if (pharmacist == null)
        //    {
        //        return NotFound("Pharmacist not found.");
        //    }

        //    // Fetch dispensed prescriptions for the logged-in pharmacist within the date range
        //    var dispensedPrescriptions = _context.DispensedPrescriptions
        //                                .Where(dp => dp.PharmacistID == pharmacist.Id
        //                                             && dp.Date >= startDate
        //                                             && dp.Date <= endDate)
        //                                .Select(dp => new
        //                                {
        //                                    dp.Date,
        //                                    PatientName = $"{dp.Patient.Name} {dp.Patient.Surname}",
        //                                    SurgeonName = $"{dp.Surgeon.Name} {dp.Surgeon.Surname}",
        //                                    dp.PrescriptionStatus,
        //                                    MedicationName = dp.Medication.MedicationName,
        //                                    dp.Quantity,
        //                                    dp.Instructions
        //                                })
        //                                .ToList();

        //    // Calculate medication summary
        //    var medicationSummary = _context.DispensedPrescriptions
        //        .Where(dp => dp.PharmacistID == pharmacist.Id
        //                     && dp.Date >= startDate
        //                     && dp.Date <= endDate)
        //        .GroupBy(dp => dp.Medication.MedicationName)
        //        .Select(g => new
        //        {
        //            MedicationName = g.Key,
        //            TotalDispensed = g.Count()
        //        })
        //        .ToList();

        //    // Pass the required data to the view
        //    ViewBag.PharmacistName = $"{pharmacist.Name} {pharmacist.Surname}";
        //    ViewBag.DispensedPrescriptions = dispensedPrescriptions;
        //    ViewBag.MedicationSummaries = medicationSummary;
        //    ViewBag.StartDate = startDate;
        //    ViewBag.EndDate = endDate;

        //    return View();
        //}


        [HttpGet("DispensedPrescriptions/Report")]
        public IActionResult Report(DateTime? startDate, DateTime? endDate)
        {
            // Default to current month if no dates are provided
            if (startDate == null)
                startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            if (endDate == null)
                endDate = DateTime.Now;
            // Adjust endDate to cover the entire day
            endDate = endDate.Value.Date.AddDays(1).AddTicks(-1);

            // Fetch the logged-in pharmacist's identity (using email)
            var userName = User.Identity.Name;

            // Find the pharmacist in the database
            var pharmacist = _context.MedicalProfessionalRecords
                                     .FirstOrDefault(m => m.EmailAddress == userName);
            if (pharmacist == null)
            {
                return NotFound("Pharmacist not found.");
            }

            // Fetch dispensed prescriptions for the logged-in pharmacist within the date range
            var dispensedPrescriptions = _context.DispensedPrescriptions
                                        .Where(dp => dp.PharmacistID == pharmacist.Id
                                                     && dp.Date >= startDate
                                                     && dp.Date <= endDate)
                                        .Select(dp => new
                                        {
                                            dp.Date,
                                            PatientName = $"{dp.Patient.Name} {dp.Patient.Surname}",
                                            SurgeonName = $"{dp.Surgeon.Name} {dp.Surgeon.Surname}",
                                            dp.PrescriptionStatus,
                                            MedicationName = dp.Medication.MedicationName,
                                            dp.Quantity,
                                            dp.Instructions
                                        })
                                        .ToList();

            // Calculate medication summary
            var medicationSummary = _context.DispensedPrescriptions
                .Where(dp => dp.PharmacistID == pharmacist.Id
                             && dp.Date >= startDate
                             && dp.Date <= endDate)
                .GroupBy(dp => dp.Medication.MedicationName)
                .Select(g => new
                {
                    MedicationName = g.Key,
                    TotalDispensed = g.Count()
                })
                .ToList();

            // Pass the required data to the view
            ViewBag.PharmacistName = $"{pharmacist.Name} {pharmacist.Surname}";
            ViewBag.DispensedPrescriptions = dispensedPrescriptions;
            ViewBag.MedicationSummaries = medicationSummary;
            ViewBag.StartDate = startDate;
            ViewBag.EndDate = endDate;

            // Debugging info to ensure data is fetched
            if (dispensedPrescriptions.Count == 0)
            {
                ViewBag.Message = "No dispensed prescriptions found within the selected date range.";
            }

            if (medicationSummary.Count == 0)
            {
                ViewBag.SummaryMessage = "No medication summary available for the selected date range.";
            }

            return View();
        }



        // GET: DispensedPrescriptions
        public async Task<IActionResult> Index()
        {
            var dispensedPrescriptions = await _context.DispensedPrescriptions
                .Include(r => r.Surgeon)
                .Include(r=>r.Patient)// Load the related surgeon
                .Include(r => r.Pharmacist)    // Load the related pharmacist
                .Include(r => r.Medication)    // Load the related medication
                .ToListAsync();

            return View(dispensedPrescriptions);
        }


        // GET: DispensedPrescriptions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dispensedPrescription = await _context.DispensedPrescriptions
                .FirstOrDefaultAsync(m => m.DispensedPrescriptionID == id);
            if (dispensedPrescription == null)
            {
                return NotFound();
            }

            return View(dispensedPrescription);
        }

        // GET: DispensedPrescriptions/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: DispensedPrescriptions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DispensedPrescriptionID,PrescriptionID,SurgeonID,PatientID,Date,PrescriptionStatus,PharmacistID,MedicationID,Quantity,Instructions")] DispensedPrescription dispensedPrescription)
        {
            if (ModelState.IsValid)
            {
                _context.Add(dispensedPrescription);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(dispensedPrescription);
        }

        // GET: DispensedPrescriptions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dispensedPrescription = await _context.DispensedPrescriptions.FindAsync(id);
            if (dispensedPrescription == null)
            {
                return NotFound();
            }
            return View(dispensedPrescription);
        }

        // POST: DispensedPrescriptions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DispensedPrescriptionID,PrescriptionID,SurgeonID,PatientID,Date,PrescriptionStatus,PharmacistID,MedicationID,Quantity,Instructions")] DispensedPrescription dispensedPrescription)
        {
            if (id != dispensedPrescription.DispensedPrescriptionID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dispensedPrescription);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DispensedPrescriptionExists(dispensedPrescription.DispensedPrescriptionID))
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
            return View(dispensedPrescription);
        }

        // GET: DispensedPrescriptions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dispensedPrescription = await _context.DispensedPrescriptions
                .FirstOrDefaultAsync(m => m.DispensedPrescriptionID == id);
            if (dispensedPrescription == null)
            {
                return NotFound();
            }

            return View(dispensedPrescription);
        }

        // POST: DispensedPrescriptions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var dispensedPrescription = await _context.DispensedPrescriptions.FindAsync(id);
            if (dispensedPrescription != null)
            {
                _context.DispensedPrescriptions.Remove(dispensedPrescription);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DispensedPrescriptionExists(int id)
        {
            return _context.DispensedPrescriptions.Any(e => e.DispensedPrescriptionID == id);
        }
    }
}
