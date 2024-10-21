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
    public class MedicalHistoriesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MedicalHistoriesController(ApplicationDbContext context)
        {
            _context = context;
        }



        public async Task<IActionResult> Index(string searchTerm)
        {
            // Build the query to include related entities
            var medicalHistories = _context.MedicalHistories
                .Include(m => m.Patient)
                .Include(m => m.MedicalHistoryAllergies)
                    .ThenInclude(mha => mha.ActiveIngredientRecords)
                .Include(m => m.MedicalHistoryMedications)
                    .ThenInclude(mhm => mhm.MedicationRecords)
                .Include(m => m.MedicalHistoryCondition)
                    .ThenInclude(mhc => mhc.Condition)
                .AsQueryable();

            // Apply search filter if needed
            if (!string.IsNullOrEmpty(searchTerm))
            {
                medicalHistories = medicalHistories.Where(m => m.Patient.PatientIDNumber.Contains(searchTerm));
            }

            // Fetch the data asynchronously and pass it to the view
            var result = await medicalHistories.ToListAsync();
            return View(result);
        }






        // GET: MedicalHistories/Details/5
        // GET: MedicalHistories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medicalHistory = await _context.MedicalHistories
                .Include(m => m.Patient)
                .Include(m => m.MedicalHistoryAllergies)
                    .ThenInclude(ma => ma.ActiveIngredientRecords) // Include related entities
                .Include(m => m.MedicalHistoryMedications)
                    .ThenInclude(mm => mm.MedicationRecords) // Include related entities
                .Include(m => m.MedicalHistoryCondition)
                    .ThenInclude(mc => mc.Condition) // Include related entities
                .FirstOrDefaultAsync(m => m.MedicalHistoryID == id);

            if (medicalHistory == null)
            {
                return NotFound();
            }

            return View(medicalHistory);
        }

        // GET: MedicalHistories/Create
        // GET: MedicalHistories/Create
        public IActionResult Create(int? id)
        {
            ViewBag.Patients = new SelectList(_context.Patients, "PatientID", "Name", id);
            ViewBag.Allergies = new SelectList(_context.ActiveIngredientRecords, "IngredientID", "IngredientName");
            ViewBag.Medications = new SelectList(_context.MedicationRecords, "MedicationID", "MedicationName");
            ViewBag.Conditions = new SelectList(_context.Condition, "ConditionID", "Diagnosis");

            ViewBag.MedicationReords = new SelectList(_context.MedicationRecords, "MedicationID", "MedicationName");
            ViewBag.MedicationRecords = new SelectList(_context.MedicationRecords, "MedicationID", "MedicationName");
            ViewBag.MedicationRecords = new SelectList(_context.MedicationRecords, "MedicationID", "MedicationName");

            ViewBag.Allergy = new SelectList(_context.Allergy, "AllergyID", "Name");
            ViewBag.Allergy = new SelectList(_context.Allergy, "AllergyID", "Name");
            ViewBag.Allergy = new SelectList(_context.Allergy, "AllergyID", "Name");

            return View();
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MedicalHistoryID,PatientID,DateRecorded,MedicalHistoryAllergies,MedicalHistoryMedications,MedicalHistoryCondition")] MedicalHistory medicalHistory, int[] selectedAllergies, int[] selectedMedications, int[] selectedConditions)
        {
            // Add selected allergies to the medical history
            if (selectedAllergies != null)
            {
                medicalHistory.MedicalHistoryAllergies = new List<MedicalHistoryAllergy>();
                foreach (var ingredientId in selectedAllergies)
                {
                    medicalHistory.MedicalHistoryAllergies.Add(new MedicalHistoryAllergy
                    {
                        IngredientID = ingredientId
                    });
                }
            }

            // Add selected medications to the medical history
            if (selectedMedications != null)
            {
                medicalHistory.MedicalHistoryMedications = new List<MedicalHistoryMedication>();
                foreach (var medicationId in selectedMedications)
                {
                    medicalHistory.MedicalHistoryMedications.Add(new MedicalHistoryMedication
                    {
                        MedicationID = medicationId
                    });
                }
            }

            // Add selected conditions to the medical history
            if (selectedConditions != null)
            {
                medicalHistory.MedicalHistoryCondition = new List<MedicalHistoryCondition>();
                foreach (var conditionId in selectedConditions)
                {
                    medicalHistory.MedicalHistoryCondition.Add(new MedicalHistoryCondition
                    {
                        ConditionID = conditionId
                    });
                }
            }

            _context.MedicalHistories.Add(medicalHistory);
            await _context.SaveChangesAsync();
            return RedirectToActionPermanent("Admitted", "Nurse");

            // If there is an error, repopulate ViewBags
            ViewBag.Patients = new SelectList(_context.Patients, "PatientID", "Name", medicalHistory.PatientID);
            ViewBag.Allergies = new SelectList(_context.ActiveIngredientRecords, "IngredientID", "IngredientName", selectedAllergies);
            ViewBag.Medications = new SelectList(_context.MedicationRecords, "MedicationID", "MedicationName", selectedMedications);
            ViewBag.Conditions = new SelectList(_context.Condition, "ConditionID", "Diagnosis", selectedConditions);
            return View(medicalHistory);
        }










        // GET: MedicalHistories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medicalHistory = await _context.MedicalHistories.FindAsync(id);
            if (medicalHistory == null)
            {
                return NotFound();
            }
            ViewBag.Patients = new SelectList(_context.Patients, "PatientID", "Name", medicalHistory.PatientID);
            return View(medicalHistory);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MedicalHistoryID,PatientID,DateRecorded")] MedicalHistory medicalHistory)
        {
            if (id != medicalHistory.MedicalHistoryID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(medicalHistory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MedicalHistoryExists(medicalHistory.MedicalHistoryID))
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
            ViewBag.Patients = new SelectList(_context.Patients, "PatientID", "Name", medicalHistory.PatientID);
            return View(medicalHistory);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medicalHistory = await _context.MedicalHistories
                .Include(m => m.Patient)
                .FirstOrDefaultAsync(m => m.MedicalHistoryID == id);
            if (medicalHistory == null)
            {
                return NotFound();
            }

            return View(medicalHistory);
        }

        // POST: MedicalHistories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var medicalHistory = await _context.MedicalHistories.FindAsync(id);
            if (medicalHistory != null)
            {
                _context.MedicalHistories.Remove(medicalHistory);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MedicalHistoryExists(int id)
        {
            return _context.MedicalHistories.Any(e => e.MedicalHistoryID == id);
        }

        //// GET: MedicalHistories/GetPatientMedicalHistory/5
        public ActionResult GetPatientMedicalHistory(int id)
        {
            // Fetch medical history with related data
            var medicalHistory = _context.MedicalHistories
                .Include(m => m.MedicalHistoryMedications)
                    .ThenInclude(medication => medication.MedicationRecords) // Load medications
                .Include(m => m.MedicalHistoryAllergies)
                    .ThenInclude(allergy => allergy.ActiveIngredientRecords) // Load allergies
                .Include(m => m.MedicalHistoryCondition)
                    .ThenInclude(condition => condition.Condition) // Load conditions
                .FirstOrDefault(m => m.PatientID == id);

            // Check if medical history was found
            if (medicalHistory == null)
            {
                // Return a not found response or error message
                return Content("Medical history not found.");
            }

            // Return the partial view with the medical history data
            return PartialView("_PatientMedicalHistory", medicalHistory);
        }


    }
}
