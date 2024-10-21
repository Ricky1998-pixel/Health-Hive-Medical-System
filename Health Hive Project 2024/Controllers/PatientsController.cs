using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Health_Hive_Project_2024.Data;
using Health_Hive_Project_2024.Models;
using Health_Hive_Project_2024.ViewModels;

namespace Health_Hive_Project_2024.Controllers
{
    public class PatientsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PatientsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Patients
        public async Task<IActionResult> Index()
        {
            return View(await _context.Patients.ToListAsync());
        }

        // GET: Patients/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var patient = await _context.Patients
                .FirstOrDefaultAsync(m => m.PatientID == id);
            if (patient == null)
            {
                return NotFound();
            }

            return View(patient);
        }
        //new new code
        //   [HttpGet]
        //   public async Task<IActionResult> SearchPatient(string patientIdNumber)
        //   {
        //       if (string.IsNullOrEmpty(patientIdNumber))
        //       {
        //           return PartialView("_PatientDetails", null);
        //       }

        //       var patient = await _context.Patients
        //           .Include(p => p.MedicalHistories)
        //    .ThenInclude(mh => mh.MedicalHistoryAllergies)
        //        .ThenInclude(a => a.ActiveIngredientRecords)
        //.Include(p => p.MedicalHistories)
        //    .ThenInclude(mh => mh.MedicalHistoryMedications)
        //        .ThenInclude(m => m.MedicationRecords)
        //.Include(p => p.MedicalHistories)
        //    .ThenInclude(mh => mh.MedicalHistoryCondition)
        //        .ThenInclude(c => c.Condition)
        //           .FirstOrDefaultAsync(p => p.PatientIDNumber == patientIdNumber);

        //       return PartialView("_PatientDetails", patient);
        //   }



        //New Testing Code

        [HttpGet]
        public async Task<IActionResult> SearchPatientById(string patientIdNumber, string medicationIds)
        {
            if (string.IsNullOrEmpty(patientIdNumber))
            {
                return PartialView("_PatientDetails", new PatientDetailsViewModel());
            }

            var patient = await _context.Patients
                .Include(p => p.MedicalHistories)
                    .ThenInclude(mh => mh.MedicalHistoryAllergies)
                        .ThenInclude(mha => mha.ActiveIngredientRecords)
                .Include(p => p.MedicalHistories)
                    .ThenInclude(mh => mh.MedicalHistoryMedications)
                        .ThenInclude(mhm => mhm.MedicationRecords)
                .Include(p => p.MedicalHistories)
                    .ThenInclude(mh => mh.MedicalHistoryCondition)
                        .ThenInclude(mhc => mhc.Condition)
                .FirstOrDefaultAsync(p => p.PatientIDNumber == patientIdNumber);

            var alerts = new List<string>();

            if (patient != null && !string.IsNullOrEmpty(medicationIds))
            {
                var selectedMedications = medicationIds.Split(',').ToList();
                var medicalHistory = patient.MedicalHistories.FirstOrDefault();

                if (medicalHistory != null)
                {
                    alerts = await CheckForAlerts(medicalHistory, selectedMedications);
                }
            }

            var viewModel = new PatientDetailsViewModel
            {
                Patient = patient,
                Alerts = alerts
            };

            return PartialView("_PatientDetails", viewModel);
        }




        // Function to check for alerts based on medical history and selected medications
        //private async Task<List<string>> CheckForAlerts(MedicalHistory medicalHistory, List<string> selectedMedications)
        //{
        //    var alerts = new List<string>();

        //    foreach (var medicationId in selectedMedications)
        //    {
        //        var medication = await _context.MedicationRecords
        //            .Include(m => m.MedicationActiveIngredients)
        //                .ThenInclude(mai => mai.ActiveIngredientRecords)
        //            .FirstOrDefaultAsync(m => m.MedicationID.ToString() == medicationId);

        //        if (medication != null)
        //        {
        //            // Check for allergies
        //            foreach (var allergy in medicalHistory.MedicalHistoryAllergies)
        //            {
        //                if (medication.MedicationActiveIngredients.Any(mai => mai.ActiveIngredientRecords.IngredientID == allergy.ActiveIngredientRecords.IngredientID))
        //                {
        //                    alerts.Add($"Allergy warning: Patient is allergic to {allergy.ActiveIngredientRecords.IngredientName}, which is present in {medication.MedicationName}.");
        //                }
        //            }

        //            // Check for contraindicated medications
        //            if (medicalHistory.MedicalHistoryMedications.Any(mhm => mhm.MedicationRecords.MedicationID.ToString() == medicationId))
        //            {
        //                alerts.Add($"Medication warning: Patient is already taking {medication.MedicationName}. Consider possible interactions.");
        //            }

        //            // Check for conditions that may conflict with selected medications
        //            //foreach (var condition in medicalHistory.MedicalHistoryCondition)
        //            //{
        //            //    var contraindicatedMedication = await _context.ContraindicatedMedications
        //            //        .FirstOrDefaultAsync(cm => cm.ConditionID == condition.ConditionID && cm.MedicationID.ToString() == medicationId);

        //            //    if (contraindicatedMedication != null)
        //            //    {
        //            //        alerts.Add($"Condition warning: Patient's condition {condition.Condition.Diagnosis} may be affected by {medication.MedicationName}.");
        //            //    }
        //            //}
        //        }
        //    }

        //    return alerts;
        //}

        private static readonly Dictionary<string, List<string>> medicationInteractions = new Dictionary<string, List<string>>
{
    { "Concerta", new List<string> { "Propofol","Migril","Lopresor","Diprivan","Cardura" } },
    { "Lopresor", new List<string> { "Hydralaz", "Cardura","Mybulen","Concerta" } },
    { "Neo-Mercazole", new List<string> { "Aspavor","Diprivan","Concerta" } },
    { "Ketalar", new List<string> { "Diprivan","Cardura" } },
    {"Compral", new List<string>{"Aspirin","Lopresor","Aspavor","Propofol"} },
            {"Migril",new List<string>{"Neo-Mercazole","Cardura","Propofol","Diprivan"} },
            {"Cardura", new List<string>{"Aspavor", "Ketalar","Concerta"} },
            {"Aspavor", new List<string>{"Mybulen","Diprivan","Propofol"} },
            {"Adco-Dol", new List<string>{"Concerta","Diprivan","Cardura"} },
            {"Mybulen", new List<string>{"Lopresor","Aspavor","Cardura","Propofol"} },
            {"Diprivan", new List<string>{"Concerta","Migril","Ketalar","Cardura","Mybulen","Adco-Dol"} },
            {"Xylocaine", new List<string>{"Propofol","Cardura","Concerta"} },
            {"Hydralaz", new List<string>{"Cardura","Concerta"} },
            {"Cotempla", new List<string>{ "Propofol", "Migril", "Lopresor", "Diprivan", "Cardura" } },
            {"Carbimazole", new List<string>{ "Doxazosin" } },
            {"Doxazosin", new List<string>{ "Doxylamine Succinate" } }
    // Add more interactions as needed
};

        private bool CheckForInteraction(string medicationA, string medicationB)
        {
            return (medicationInteractions.ContainsKey(medicationA) && medicationInteractions[medicationA].Contains(medicationB)) ||
                   (medicationInteractions.ContainsKey(medicationB) && medicationInteractions[medicationB].Contains(medicationA));
        }

        private async Task<List<string>> CheckForAlerts(MedicalHistory medicalHistory, List<string> selectedMedications)
        {
            var alerts = new List<string>();
            var checkedMedications = new List<MedicationRecords>();

            foreach (var medicationId in selectedMedications)
            {
                var medication = await _context.MedicationRecords
                    .Include(m => m.MedicationActiveIngredients)
                        .ThenInclude(mai => mai.ActiveIngredientRecords)
                    .FirstOrDefaultAsync(m => m.MedicationID.ToString() == medicationId);

                if (medication != null)
                {
                    // Check for allergies
                    foreach (var allergy in medicalHistory.MedicalHistoryAllergies)
                    {
                        if (medication.MedicationActiveIngredients.Any(mai => mai.ActiveIngredientRecords.IngredientID == allergy.ActiveIngredientRecords.IngredientID))
                        {
                            alerts.Add($"Allergy warning: Patient is allergic to {allergy.ActiveIngredientRecords.IngredientName}, which is present in {medication.MedicationName}.");
                        }
                    }

                    // Check for contraindicated medications
                    if (medicalHistory.MedicalHistoryMedications.Any(mhm => mhm.MedicationRecords.MedicationID.ToString() == medicationId))
                    {
                        alerts.Add($"Medication warning: Patient is already taking {medication.MedicationName}. Consider possible interactions.");
                    }

                    // Check for interactions with other selected medications
                    foreach (var checkedMed in checkedMedications)
                    {
                        if (CheckForInteraction(medication.MedicationName, checkedMed.MedicationName))
                        {
                            alerts.Add($"Interaction alert: Potential interaction between {medication.MedicationName} and {checkedMed.MedicationName}. Consult a healthcare professional for guidance.");
                        }
                    }

                    // Check for interactions with medications in medical history
                    foreach (var historyMed in medicalHistory.MedicalHistoryMedications)
                    {
                        if (CheckForInteraction(medication.MedicationName, historyMed.MedicationRecords.MedicationName))
                        {
                            alerts.Add($"Interaction alert: Potential interaction between {medication.MedicationName} and {historyMed.MedicationRecords.MedicationName} (in patient's history). Consult a healthcare professional for guidance.");
                        }
                    }

                    checkedMedications.Add(medication);
                }
            }

            return alerts;
        }















        //New Code
        [HttpGet]
        [Route("api/patients/{patientIDNumber}")]
        public async Task<IActionResult> GetPatientDetails(string patientIDNumber)
        {
            var patient = await _context.Patients.FirstOrDefaultAsync(p => p.PatientIDNumber == patientIDNumber);

            if (patient == null)
            {
                return NotFound(); // Patient not found
            }

            return Ok(patient); // Return patient details as JSON
        }


        [HttpPost]
        public IActionResult SearchPatientIndex(string id)
        {
            var patients = _context.Patients
                .Where(p => p.PatientIDNumber.Contains(id))
                .ToList();

            return PartialView("_PatientTableBody", patients);
        }



        // GET: Patients/Create
        public IActionResult Create(string patientID)
        {
            var model = new Patient();

            // If a patientID is passed as a query parameter, set it in the model
            if (!string.IsNullOrEmpty(patientID))
            {
                model.PatientIDNumber = patientID;
            }

            return View(model);
        }

        // POST: Patients/Create
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PatientID,PatientIDNumber,Name,Surname,Address,ContactNumber,EmailAddress,DateOfBirth,Gender")] Patient patient)
        {
            
                _context.Add(patient);
                await _context.SaveChangesAsync();

                // For AJAX requests, return JSON response with the newly created patient's ID
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new { success = true, patientID = patient.PatientID });
                }

                // For normal requests, redirect to Index
                return RedirectToAction(nameof(Index));
            

            // For AJAX requests, return JSON response with errors
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new { success = false });
            }

            // For normal requests, return the view with model errors
            return View(patient);
        }





        // GET: Patients/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var patient = await _context.Patients.FindAsync(id);
            if (patient == null)
            {
                return NotFound();
            }
            return View(patient);
        }

        // POST: Patients/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PatientID,PatientIDNumber,Name,Surname,Address,ContactNumber,EmailAddress,DateOfBirth,Gender")] Patient patient)
        {
            if (id != patient.PatientID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(patient);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PatientExists(patient.PatientID))
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
            return View(patient);
        }

        // GET: Patients/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var patient = await _context.Patients
                .FirstOrDefaultAsync(m => m.PatientID == id);
            if (patient == null)
            {
                return NotFound();
            }

            return View(patient);
        }

        // POST: Patients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var patient = await _context.Patients.FindAsync(id);
            if (patient != null)
            {
                _context.Patients.Remove(patient);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PatientExists(int id)
        {
            return _context.Patients.Any(e => e.PatientID == id);
        }

        public IActionResult Index1()
        {
            return View();
        }
    }
}
