using Health_Hive_Project_2024.Data;
using Health_Hive_Project_2024.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Health_Hive_Project_2024.Controllers
{
    public class PostSurgeryController : Controller
    {

        private readonly ApplicationDbContext _context;

        public PostSurgeryController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            // Retrieve patients who exist in SurgeryBooking only
            var patientsWithSurgery = await _context.Patients
                .Where(p => _context.PatientAdmissions.Any(sb => sb.PatientID == p.PatientID))
                .ToListAsync();

            return View(patientsWithSurgery);
        }
        public IActionResult ChangeStatus(int id, int PatientID, OrderStatus newStatus)
        {
            var booking = _context.GetMedicationOrder.Find(id);
            if (booking != null)
            {
                booking.Status = newStatus;
                booking.IsReceived = true;
                _context.SaveChanges();


            }
            return RedirectToAction("Order", "PostSurgery", new { id = PatientID });
        }

        [HttpGet]
        public async Task<IActionResult> Order(int id) // Assuming PatientID is an int
        {
            // Fetch orders for the patient where the status is 'Dispensed' or 'Received'
            var dispensedOrders = await _context.GetMedicationOrder
                                                 .Include(o => o.Patient)
                                                 .Include(o => o.GetMedicationData) // Assuming these relationships exist
                                                 .Where(o => o.Patient.PatientID == id &&
                                                          (o.Status == OrderStatus.Dispensed ||
                                                           o.Status == OrderStatus.Received))
                                                 .ToListAsync();

            // Return the view with the list of dispensed orders
            return View(dispensedOrders);
        }

        [HttpGet]
        public IActionResult GetInfor(string searchText)
        {
            var filteredPatients = _context.Patients
                 .Where(p => _context.PatientAdmissions.Any(sb => sb.PatientID == p.PatientID))
                .Where(p => p.PatientIDNumber.Contains(searchText))
                .ToList();

            // Return a partial view to dynamically replace the table rows
            return PartialView("_PatientRow", filteredPatients);
        }

    }
}
