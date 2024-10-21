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
    public class CompleteSurgeriesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CompleteSurgeriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: CompleteSurgeries
        //public async Task<IActionResult> Index()
        //{

        //    var applicationDbContext = _context.CompleteSurgeries.Include(c => c.Anaesthesiologist).Include(c => c.Patient).Include(c => c.Surgeon).Include(c => c.Theatre);
        //    return View(await applicationDbContext.ToListAsync());
        //}

        // GET: CompleteSurgeries
        public async Task<IActionResult> Index()
        {
            // Get the logged-in user's email address
            var userEmail = User.Identity.Name;

            // Check if the user is an Anesthesiologist
            var anesthesiologist = await _context.MedicalProfessionalRecords
                                           .FirstOrDefaultAsync(p => p.EmailAddress == userEmail && p.Specialization == SpecializationType.Anesthesiologist);

            // Check if the user is a Surgeon
            var surgeon = await _context.MedicalProfessionalRecords
                                           .FirstOrDefaultAsync(p => p.EmailAddress == userEmail && p.Specialization == SpecializationType.Surgeon);

            // Set the ViewBag flags based on the user's role
            ViewBag.IsAnesthesiologist = anesthesiologist != null;
            ViewBag.IsSurgeon = surgeon != null;

            // Fetch all CompleteSurgeries and related data
            IQueryable<CompleteSurgery> completeSurgeries = _context.CompleteSurgeries
                .Include(c => c.Anaesthesiologist)
                .Include(c => c.Patient)
                .Include(c => c.Surgeon)
                .Include(c => c.Theatre)
                .Include(sb => sb.SurgeryBookingTreatments)
                .ThenInclude(t => t.TreatmentRecords);

            // If the user is a Surgeon, filter by the surgeon's ID
            if (surgeon != null)
            {
                completeSurgeries = completeSurgeries.Where(c => c.Surgeon.Id == surgeon.Id);
            }
            // If the user is an Anesthesiologist, filter by the anesthesiologist's ID
            else if (anesthesiologist != null)
            {
                completeSurgeries = completeSurgeries.Where(c => c.Anaesthesiologist.Id == anesthesiologist.Id);
            }

            // Return the filtered (or unfiltered) results to the view
            return View(await completeSurgeries.ToListAsync());
        }


        [HttpGet("CompleteSurgeries/CompleteSurgeryReports")]
        public IActionResult CompleteSurgeryReports(DateTime? startDate, DateTime? endDate)
        {
            if (startDate == null)
                startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            if (endDate == null)
                endDate = DateTime.Now;
            endDate = endDate.Value.Date.AddDays(1).AddTicks(-1);

            var userName = User.Identity.Name;
            var surgeon = _context.MedicalProfessionalRecords
                                  .FirstOrDefault(m => m.UserName == userName);
            if (surgeon == null)
            {
                return NotFound("Surgeon not found.");
            }

            var completeSurgeries = _context.CompleteSurgeries
                                    .Where(s => s.SurgeonID == surgeon.Id
                                                && s.SurgeryDate >= startDate
                                                && s.SurgeryDate <= endDate)
                                    .Select(s => new
                                    {
                                        s.SurgeryDate,
                                        PatientName = $"{s.Patient.Name} {s.Patient.Surname}",
                                        s.SurgeryID // We need this to link with SurgeryBookingTreatment
                                    })
                                    .ToList();

            var surgeryIds = completeSurgeries.Select(s => s.SurgeryID).ToList();

            var treatmentsForSurgeries = _context.SurgeryBookingTreatments
                .Where(sbt => surgeryIds.Contains(sbt.SurgeryID))
                .Select(sbt => new
                {
                    sbt.SurgeryID,
                    TreatmentCode = sbt.TreatmentRecords.TreatmentCode
                })
                .ToList();

            var surgeriesWithTreatments = completeSurgeries.Select(s => new
            {
                s.SurgeryDate,
                s.PatientName,
                TreatmentCodes = treatmentsForSurgeries
                    .Where(t => t.SurgeryID == s.SurgeryID)
                    .Select(t => t.TreatmentCode)
                    .ToList()
            }).ToList();

            var treatmentSummary = treatmentsForSurgeries
                .GroupBy(t => t.TreatmentCode)
                .Select(g => new
                {
                    TreatmentCode = g.Key,
                    TotalSurgeries = g.Count()
                })
                .ToList();

            ViewBag.SurgeonName = $"{surgeon.Name} {surgeon.Surname}";
            ViewBag.Surgeries = surgeriesWithTreatments;
            ViewBag.TreatmentSummaries = treatmentSummary;
            ViewBag.StartDate = startDate;
            ViewBag.EndDate = endDate;
            return View();
        }





        // GET: CompleteSurgeries/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var completeSurgery = await _context.CompleteSurgeries
                .Include(c => c.Anaesthesiologist)
                .Include(c => c.Patient)
                .Include(c => c.Surgeon)
                .Include(c => c.Theatre)
                .FirstOrDefaultAsync(m => m.CompleteSurgeryID == id);
            if (completeSurgery == null)
            {
                return NotFound();
            }

            return View(completeSurgery);
        }

        // GET: CompleteSurgeries/Create
        public IActionResult Create()
        {
            ViewData["AnaesthesiologistID"] = new SelectList(_context.MedicalProfessionalRecords, "Id", "Id");
            ViewData["PatientID"] = new SelectList(_context.Patients, "PatientID", "Address");
            ViewData["SurgeonID"] = new SelectList(_context.MedicalProfessionalRecords, "Id", "Id");
            ViewData["TheatreID"] = new SelectList(_context.OperatingTheatreRecords, "TheatreID", "TheatreName");
            return View();
        }

        // POST: CompleteSurgeries/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CompleteSurgeryID,SurgeryID,SurgeonID,PatientID,AnaesthesiologistID,TheatreID,SurgeryDate,Session")] CompleteSurgery completeSurgery)
        {
            if (ModelState.IsValid)
            {
                _context.Add(completeSurgery);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AnaesthesiologistID"] = new SelectList(_context.MedicalProfessionalRecords, "Id", "Id", completeSurgery.AnaesthesiologistID);
            ViewData["PatientID"] = new SelectList(_context.Patients, "PatientID", "Address", completeSurgery.PatientID);
            ViewData["SurgeonID"] = new SelectList(_context.MedicalProfessionalRecords, "Id", "Id", completeSurgery.SurgeonID);
            ViewData["TheatreID"] = new SelectList(_context.OperatingTheatreRecords, "TheatreID", "TheatreName", completeSurgery.TheatreID);
            return View(completeSurgery);
        }

        // GET: CompleteSurgeries/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var completeSurgery = await _context.CompleteSurgeries.FindAsync(id);
            if (completeSurgery == null)
            {
                return NotFound();
            }
            ViewData["AnaesthesiologistID"] = new SelectList(_context.MedicalProfessionalRecords, "Id", "Id", completeSurgery.AnaesthesiologistID);
            ViewData["PatientID"] = new SelectList(_context.Patients, "PatientID", "Address", completeSurgery.PatientID);
            ViewData["SurgeonID"] = new SelectList(_context.MedicalProfessionalRecords, "Id", "Id", completeSurgery.SurgeonID);
            ViewData["TheatreID"] = new SelectList(_context.OperatingTheatreRecords, "TheatreID", "TheatreName", completeSurgery.TheatreID);
            return View(completeSurgery);
        }

        // POST: CompleteSurgeries/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CompleteSurgeryID,SurgeryID,SurgeonID,PatientID,AnaesthesiologistID,TheatreID,SurgeryDate,Session")] CompleteSurgery completeSurgery)
        {
            if (id != completeSurgery.CompleteSurgeryID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(completeSurgery);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CompleteSurgeryExists(completeSurgery.CompleteSurgeryID))
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
            ViewData["AnaesthesiologistID"] = new SelectList(_context.MedicalProfessionalRecords, "Id", "Id", completeSurgery.AnaesthesiologistID);
            ViewData["PatientID"] = new SelectList(_context.Patients, "PatientID", "Address", completeSurgery.PatientID);
            ViewData["SurgeonID"] = new SelectList(_context.MedicalProfessionalRecords, "Id", "Id", completeSurgery.SurgeonID);
            ViewData["TheatreID"] = new SelectList(_context.OperatingTheatreRecords, "TheatreID", "TheatreName", completeSurgery.TheatreID);
            return View(completeSurgery);
        }

        // GET: CompleteSurgeries/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var completeSurgery = await _context.CompleteSurgeries
                .Include(c => c.Anaesthesiologist)
                .Include(c => c.Patient)
                .Include(c => c.Surgeon)
                .Include(c => c.Theatre)
                .FirstOrDefaultAsync(m => m.CompleteSurgeryID == id);
            if (completeSurgery == null)
            {
                return NotFound();
            }

            return View(completeSurgery);
        }

        // POST: CompleteSurgeries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var completeSurgery = await _context.CompleteSurgeries.FindAsync(id);
            if (completeSurgery != null)
            {
                _context.CompleteSurgeries.Remove(completeSurgery);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CompleteSurgeryExists(int id)
        {
            return _context.CompleteSurgeries.Any(e => e.CompleteSurgeryID == id);
        }
    }
}
