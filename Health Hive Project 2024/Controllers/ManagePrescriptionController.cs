using Health_Hive_Project_2024.Data;
using Health_Hive_Project_2024.Data.ViewModels;
using Health_Hive_Project_2024.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Security.Cryptography;

namespace Health_Hive_Project_2024.Controllers
{
    public class ManagePrescriptionController : Controller
    {

        private readonly ApplicationDbContext _context;
        private readonly UserManager<MedicalProfessionalRecords> _userManager;
        public ManagePrescriptionController(ApplicationDbContext context, UserManager<MedicalProfessionalRecords> userManager)
        {
            _context = context;
            this._userManager = userManager;
        }

        public async Task<IActionResult> Rejected(string patientId)
        {
            var userEmail = User.Identity.Name;

            // Check if the user is a Pharmacist
            var pharmacist = await _context.MedicalProfessionalRecords
                                           .FirstOrDefaultAsync(p => p.EmailAddress == userEmail && p.Specialization == SpecializationType.Pharmacist);

            if (pharmacist != null)
            {
                ViewBag.IsPharmacist = true;
            }

            // Check if the user is a Surgeon
            var anae = await _context.MedicalProfessionalRecords
                                           .FirstOrDefaultAsync(p => p.EmailAddress == userEmail && p.Specialization == SpecializationType.Anesthesiologist);

            if (anae != null)
            {
                ViewBag.IsSurgeon = true;
            }

            // Retrieve prescription records
            IQueryable<OrderMedications> query = _context.GetMedicationOrder
                .Include(p => p.GetMedicationData)
                .ThenInclude(pm => pm.Medication)
                .Include(p => p.Patient)
                .Include(p => p.GetAnaesthesiologist);

            // Adjust query based on role (Pharmacist or Surgeon)
            if (pharmacist != null)
            {
                // If the user is a pharmacist, filter by PharmacistID
                query = query.Where(p => p.PharmacistID == pharmacist.Id);
            }
            else if (anae != null)
            {
                // If the user is a surgeon, filter by SurgeonID
                query = query.Where(p => p.Id == anae.Id);
            }

            // Apply patient ID filter if provided
            if (!string.IsNullOrEmpty(patientId))
            {
                query = query.Where(p => p.Patient.PatientIDNumber == patientId);
            }

            var prescriptions = await query.ToListAsync();

            if (!prescriptions.Any() && !string.IsNullOrEmpty(patientId))
            {
                ViewBag.Message = "No prescriptions found for this Patient ID Number.";
            }

            return View(prescriptions);
        }
        // GET: PrescriptionRecords/Index
        public async Task<IActionResult> Index(string patientId)
        {
            var userEmail = User.Identity.Name;

            // Check if the user is a Pharmacist
            var pharmacist = await _context.MedicalProfessionalRecords
                                           .FirstOrDefaultAsync(p => p.EmailAddress == userEmail && p.Specialization == SpecializationType.Pharmacist);

            if (pharmacist != null)
            {
                ViewBag.IsPharmacist = true;
            }

            // Check if the user is a Surgeon
            var anae = await _context.MedicalProfessionalRecords
                                           .FirstOrDefaultAsync(p => p.EmailAddress == userEmail && p.Specialization == SpecializationType.Anesthesiologist);

            if (anae != null)
            {
                ViewBag.IsSurgeon = true;
            }

            // Retrieve prescription records
            IQueryable<OrderMedications> query = _context.GetMedicationOrder
                .Include(p => p.GetMedicationData)
                .ThenInclude(pm => pm.Medication)
                .Include(p => p.Patient)
                .Include(p => p.GetAnaesthesiologist);

            // Adjust query based on role (Pharmacist or Surgeon)
            if (pharmacist != null)
            {
                // If the user is a pharmacist, filter by PharmacistID
                query = query.Where(p => p.PharmacistID == pharmacist.Id);
            }
            else if (anae != null)
            {
                // If the user is a surgeon, filter by SurgeonID
                query = query.Where(p => p.Id == anae.Id);
            }

            // Apply patient ID filter if provided
            if (!string.IsNullOrEmpty(patientId))
            {
                query = query.Where(p => p.Patient.PatientIDNumber == patientId);
            }

            var prescriptions = await query.ToListAsync();

            if (!prescriptions.Any() && !string.IsNullOrEmpty(patientId))
            {
                ViewBag.Message = "No prescriptions found for this Patient ID Number.";
            }

            return View(prescriptions);
        }

        public IActionResult ChangeStatus(int id, int PatientID, OrderStatus newStatus)
        {
            var booking = _context.GetMedicationOrder.Find(id);
            if (booking != null)
            {
                booking.Status = newStatus;
                booking.IsReceived = false;
                _context.SaveChanges();
                var notification = new NortificationModel
                {
                    Id = User.FindFirstValue(ClaimTypes.NameIdentifier), // Set the ID of the logged-in user
                    Description = $"Order '{booking.PrescriptionOrder}' has been Dispensed. You can now Receive it!", // Description of the vital
                    TimeStamp = DateTime.Now // Set the current timestamp
                };
                _context.GetNortifications.Add(notification);
                _context.SaveChangesAsync();
                TempData["SuccessMessage"] = $" Order '{booking.PrescriptionOrder}' has been dispensed!";
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult RejectOrder(int OrderMedicationID, string Instructions)
        {
            var order = _context.GetMedicationOrder.Find(OrderMedicationID);
            if (order != null)
            {
                order.Status = OrderStatus.Rejected;
                order.IsReceived = false;
                _context.SaveChangesAsync();


                order.Instructions = Instructions;
                _context.SaveChangesAsync();
                var ord = _context.GetMedicationOrder
                       .FirstOrDefault(o => o.OrderMedicationID == OrderMedicationID);
                var notification = new NortificationModel
                {
                    Id = User.FindFirstValue(ClaimTypes.NameIdentifier), // Set the ID of the logged-in user
                    Description = $"Order '{ord.PrescriptionOrder}' has been  Rejected!", // Description of the vital
                    TimeStamp = DateTime.Now // Set the current timestamp
                };
                _context.GetNortifications.Add(notification);
                _context.SaveChangesAsync();


                if (ord != null)
                {
                    // Set success message with the specific prescription order
                    TempData["SuccessMessage"] = $"Medication Order successfully Rejected on {ord.PrescriptionOrder}!";

                }
                else
                {
                    TempData["SuccessMessage"] = "Medication added successfully, but the associated order was not found.";
                }


            }

            // Redirect back to the same patient's details or order view
            return RedirectToAction("Index", "ManagePrescription");
        }

        [HttpGet]
        public async Task<IActionResult> Dispensed(string patientId)
        {
            var userEmail = User.Identity.Name;

            // Check if the user is a Pharmacist
            var pharmacist = await _context.MedicalProfessionalRecords
                                           .FirstOrDefaultAsync(p => p.EmailAddress == userEmail && p.Specialization == SpecializationType.Pharmacist);

            if (pharmacist != null)
            {
                ViewBag.IsPharmacist = true;
            }

            // Check if the user is a Surgeon
            var anae = await _context.MedicalProfessionalRecords
                                           .FirstOrDefaultAsync(p => p.EmailAddress == userEmail && p.Specialization == SpecializationType.Anesthesiologist);

            if (anae != null)
            {
                ViewBag.IsSurgeon = true;
            }

            // Retrieve prescription records
            IQueryable<OrderMedications> query = _context.GetMedicationOrder
                .Include(p => p.GetMedicationData)
                .ThenInclude(pm => pm.Medication)
                .Include(p => p.Patient)
                .Include(p => p.GetAnaesthesiologist);

            // Adjust query based on role (Pharmacist or Surgeon)
            if (pharmacist != null)
            {
                // If the user is a pharmacist, filter by PharmacistID
                query = query.Where(p => p.PharmacistID == pharmacist.Id);
            }
            else if (anae != null)
            {
                // If the user is a surgeon, filter by SurgeonID
                query = query.Where(p => p.Id == anae.Id);
            }

            // Apply patient ID filter if provided
            if (!string.IsNullOrEmpty(patientId))
            {
                query = query.Where(p => p.Patient.PatientIDNumber == patientId);
            }

            var prescriptions = await query.ToListAsync();

            if (!prescriptions.Any() && !string.IsNullOrEmpty(patientId))
            {
                ViewBag.Message = "No prescriptions found for this Patient ID Number.";
            }

            return View(prescriptions);
        }

        public async Task<IActionResult> GetVitals(int patientId)
        {
            var vitals = await _context.GetVitalModels
                                       .Where(v => v.PatientID == patientId)
                                       .Include(v => v.GetNormalRange) // Include related data
                                       .ToListAsync();

            return PartialView("_VitalHistoryPartial", vitals);
        }

        public async Task<IActionResult> GetMedications(int patientId)
        {
            var medications = await _context.PatientMedications
                                            .Where(pm => pm.PatientID == patientId)
                                            .Include(pm => pm.Medication) // Include related medication data
                                            .ToListAsync();

            return PartialView("_MedicationHistoryPartial", medications);
        }

        public async Task<IActionResult> GetAllergies(int patientId)
        {
            var allergies = await _context.Allergies
                                          .Where(a => a.PatientID == patientId)
                                          .Include(a => a.Ingredient) // Include related ingredient data
                                          .ToListAsync();

            return PartialView("_AllergiesHistoryPartial", allergies);
        }

        [HttpPost]
        public IActionResult SearchPrescription(string patientId)
        {
            return RedirectToAction("Index", new { patientId });
        }

        [HttpPost]
        public IActionResult SearchId(string patientId)
        {
            return RedirectToAction("Dispensed", new { patientId });
        }

        [HttpPost]
        public IActionResult Search(string patientId)
        {
            return RedirectToAction("Rejected", new { patientId });
        }
    }
}
