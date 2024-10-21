using Health_Hive_Project_2024.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Health_Hive_Project_2024.Controllers
{
    public class BookedPatientController : Controller
    {

        private readonly ApplicationDbContext _context;

        public BookedPatientController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string? searchId)
        {
            ViewData["CurrentFilter"] = searchId;

            // Retrieve the list of booking IDs that exist in PatientAdmission
            var admittedBookingIds = _context.PatientAdmissions
                .Select(pa => pa.SurgeryBID)
                .ToHashSet();  // Efficient for lookups

            // Fetch all surgery bookings, excluding those found in PatientAdmission
            var solutions = _context.SurgeryBooking
                .Include(sb => sb.Patient)
                .Include(sb => sb.SurgeryBookingTreatments)
                .Include(sb => sb.Anaesthesiologist)
                .Include(sb => sb.Surgeon)
                .Where(sb => !admittedBookingIds.Contains(sb.SurgeryID));  // Exclude admitted bookings

            // If a search ID is provided, filter by PatientIDNumber
            if (!string.IsNullOrEmpty(searchId))
            {
                solutions = solutions.Where(sb => sb.Patient.PatientIDNumber.Contains(searchId));
            }

            // Load MedicationRecords to ViewBag
            ViewBag.MedicationRecords = await _context.MedicationRecords.ToListAsync();

            // Execute the query and return the view
            return View(await solutions.ToListAsync());
        }


        public async Task<IActionResult> GetTreatmentCode(int surgeryId)
        {
            if (surgeryId == 0)
            {
                return BadRequest("Invalid Surgery ID.");
            }

            var treatmentCode = await _context.SurgeryBookingTreatments
                                              .Where(s => s.SurgeryID == surgeryId)
                                              .Include(s => s.TreatmentRecords)
                                             .ToListAsync();

            if (treatmentCode == null)
            {
                return NotFound("Treatment code not found.");
            }

            return PartialView("_TreatmentCodePartial", treatmentCode);
        }

    }
}
