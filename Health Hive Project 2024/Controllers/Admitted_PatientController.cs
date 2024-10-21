using Health_Hive_Project_2024.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Health_Hive_Project_2024.Controllers
{
    public class Admitted_PatientController : Controller
    {

        private readonly ApplicationDbContext _context;

        public Admitted_PatientController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index(string? searchId)
        {
            ViewData["CurrentFilter"] = searchId;

            // Get only admitted patients with DischargeDate == null
            var solutions = _context.PatientAdmissions
                .Include(a => a.Patient)
                .Include(a => a.Bed)
                .Include(a => a.Ward)
                .Include(a => a.Nurse)
                .Where(a => a.DischargeDate == null); // Filter for admitted patients only

            // Apply search filter if provided
            if (!string.IsNullOrEmpty(searchId))
            {
                solutions = solutions.Where(b => b.Patient.PatientIDNumber.Contains(searchId));
            }

            // Execute query asynchronously
            var admittedPatients = await solutions.ToListAsync();

            // Retrieve medication records
            var medicationRecords = await _context.MedicationRecords.ToListAsync();
            ViewBag.MedicationRecords = medicationRecords;

            return View(admittedPatients);
        }

    }
}
