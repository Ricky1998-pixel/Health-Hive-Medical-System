using Health_Hive_Project_2024.Data;
using Health_Hive_Project_2024.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;


namespace Health_Hive_Project_2024.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context; // Add this line

        private readonly UserManager<MedicalProfessionalRecords> _userManager;
        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, UserManager<MedicalProfessionalRecords> userManager)
        {
            _logger = logger;
            _context = context;

            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        // Admin Dashboard Action
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AdminDashboard()
        {
            // Get the email address of the logged-in user
            var userEmail = User.Identity.Name;

            // Find the MedicalProfessionalRecords for the logged-in surgeon
            var admin = await _context.MedicalProfessionalRecords
                .FirstOrDefaultAsync(p => p.Email == userEmail && p.Specialization == SpecializationType.Admin);

            if (admin == null)
            {
                // Handle case where the Surgeon is not found or not logged in as a surgeon
                return RedirectToAction("AccessDenied", "Account");
            }

            // Use the Id from IdentityUser as the MedicalProfessional ID
            var adminId = admin.Id;

            // Get the count of MedicalProfessionalRecords
            var medicalProfessionalsCount = _context.MedicalProfessionalRecords.Count();

            // Pass the count to the view
            ViewData["MedicalProfessionalsCount"] = medicalProfessionalsCount;

            // Pass the surgeon's name and surname to the view
            ViewData["AdminName"] = admin.Name;
            ViewData["AdminSurname"] = admin.Surname;
            return View();
        }

        // Nurse Dashboard Action
        [Authorize(Roles = "Nurse")]
        public IActionResult NurseDashboard()
        {
            return RedirectToAction("Index","Nurse");
        }

        // Surgeon Dashboard Action
        [Authorize(Roles = "Surgeon")]
        public async Task<IActionResult> SurgeonDashboard()
        {
            // Get the email address of the logged-in user
            var userEmail = User.Identity.Name;

            // Find the MedicalProfessionalRecords for the logged-in surgeon
            var surgeon = await _context.MedicalProfessionalRecords
                .FirstOrDefaultAsync(p => p.Email == userEmail && p.Specialization == SpecializationType.Surgeon);

            if (surgeon == null)
            {
                // Handle case where the Surgeon is not found or not logged in as a surgeon
                return RedirectToAction("AccessDenied", "Account");
            }

            // Use the Id from IdentityUser as the MedicalProfessional ID
            var surgeonId = surgeon.Id;

            // Get the count of rejected prescriptions for the logged-in surgeon
            var rejectedPrescriptionsCount = await _context.RejectedPrescriptions
                .CountAsync(p => p.SurgeonID == surgeonId);

            // Get the count of dispensed prescriptions for the logged-in surgeon
            var dispensedPrescriptionsCount = await _context.DispensedPrescriptions
                .CountAsync(p => p.SurgeonID == surgeonId);

            // Pass the counts to the view
            ViewData["RejectedPrescriptionsCount"] = rejectedPrescriptionsCount;
            ViewData["DispensedPrescriptionsCount"] = dispensedPrescriptionsCount;

            // Get the count of patients for all records without filtering
            var patientsCount = await _context.Patients.CountAsync();

            // Pass the count to the view
            ViewData["PatientsCount"] = patientsCount;

            // Get the count of surgery bookings for the logged-in surgeon
            var surgeryBookingsCount = await _context.SurgeryBooking
                .CountAsync(p => p.SurgeonID == surgeonId);

            // Pass the count to the view
            ViewData["SurgeryBookingsCount"] = surgeryBookingsCount;

            // Pass the surgeon's name and surname to the view
            ViewData["SurgeonName"] = surgeon.Name;
            ViewData["SurgeonSurname"] = surgeon.Surname;

            return View();
        }



        //Pharmacist Dashboard Action
        [Authorize(Roles = "Pharmacist")]
        public async Task<IActionResult> PharmacistDashboard()
        {
            // Get the email address of the logged-in user
            var userEmail = User.Identity.Name;

            // Find the MedicalProfessionalRecords for the logged-in pharmacist
            var pharmacist = await _context.MedicalProfessionalRecords
                .FirstOrDefaultAsync(p => p.Email == userEmail && p.Specialization == SpecializationType.Pharmacist);

            if (pharmacist == null)
            {
                // Handle case where the Pharmacist is not found or not logged in as a pharmacist
                return RedirectToAction("AccessDenied", "Account");
            }

            // Use the Id from IdentityUser as the MedicalProfessional ID
            var pharmacistId = pharmacist.Id;

            // Get the count of PrescriptionRecords for the logged-in pharmacist
            var prescriptionRecordsCount = await _context.PrescriptionRecords
                .CountAsync(p => p.PharmacistID == pharmacistId);

            // Pass the count to the view
            ViewData["PrescriptionRecordsCount"] = prescriptionRecordsCount;

            // Get the count of MedicationRecords for all pharmacists
            var medicationRecordsCount = await _context.MedicationRecords.CountAsync();

            // Pass the count to the view
            ViewData["MedicationRecordsCount"] = medicationRecordsCount;

            // Pass the pharmacist's name and surname to the view
            ViewData["PharmacistName"] = pharmacist.Name;
            ViewData["PharmacistSurname"] = pharmacist.Surname;

            return View();
        }


        //Anaesthesiologist
        [Authorize(Roles = "Anesthesiologist")]
        public async Task<IActionResult> AnaesthesiologistDashboard() 
        {

            var userI = await this._userManager.GetUserAsync(User);
            string email = userI.Email;
            string firstName = userI.Name;
           
            ViewData["Name"] = firstName;
       
            return View();
        }
    }
}
