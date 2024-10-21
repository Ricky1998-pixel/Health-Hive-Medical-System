using Health_Hive_Project_2024.Data;
using Health_Hive_Project_2024.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Health_Hive_Project_2024.Controllers
{
    public class PatientRecordController : Controller
    {

        private readonly ApplicationDbContext _context;

        public PatientRecordController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }
     
        [HttpGet]
        public async Task<IActionResult> Information(int id)  // Make 'id' nullable
        {



            // Attempt to retrieve the patient with the provided ID
            var patient = await _context.Patients.FirstOrDefaultAsync(p => p.PatientID == id);


            if (patient == null)
            {
                return NotFound(); // Handle the case where the patient is not found
            }
            var medicalHistory = await _context.MedicalHistories
      .Include(m => m.MedicalHistoryAllergies)
          .ThenInclude(a => a.ActiveIngredientRecords)
      .Include(m => m.MedicalHistoryCondition)
          .ThenInclude(c => c.Condition)
      .Include(m => m.MedicalHistoryMedications)
          .ThenInclude(mr => mr.MedicationRecords)
      .FirstOrDefaultAsync(m => m.PatientID == id);

            // If patient is found, proceed with loading the other details
            var PatientDetail = new PatientDetailJoinedModel
            {
                Patient = patient,
                PatientID = id,
                MedicalHistory = medicalHistory,
                MedicationOrders = await _context.GetMedicationOrder
                    .Where(mo => mo.PatientID == id).Include(a => a.GetMedicationData).ToListAsync(),
                dataStores = await _context.GetMedicationData
                    .Where(mo => mo.GetOrder.PatientID == id).Include(a => a.GetOrder).Include(p => p.Medication).ToListAsync(),
                MedicationRecords = _context.MedicationRecords.ToList(),

                GetVitals = await _context.PatientVitals
                    .Where(mo => mo.PatientID == id).Include(a => a.Vitals).Include(p => p.Patient).ToListAsync(),
                GetVitalsList  = await _context.Vitals
                    .ToListAsync(),
                GetAllergies = await _context.Allergies
                    .Include(a => a.Ingredient).Where(mo => mo.PatientID == id).ToListAsync(),
                //GetCondition = await _context.Condition
                    //.Where(mo => mo.PatientID == id).Include(p => p.Patient).ToListAsync(),
                GetCurrentMed = await _context.PatientMedications
                    .Where(mo => mo.PatientID == id).Include(p => p.Patient).ToListAsync(),
                GetAdmission = await _context.PatientAdmissions
                    .Where(mo => mo.PatientID == id)
                    .Include(p => p.Patient)
                    .Include(p => p.Ward)
                    .Include(p => p.Bed).ToListAsync(),
            };

            return View(PatientDetail);
        }





    }
}
