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
    public class PrescriptionRecordsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PrescriptionRecordsController(ApplicationDbContext context)
        {
            _context = context;
        }



        // Prescription Report code
        // Prescription Report code
        [HttpGet("PrescriptionRecords/Report")]
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

            // Fetch prescriptions for the logged-in pharmacist within the date range
            var prescriptions = _context.PrescriptionRecords
                                        .Where(p => p.PharmacistID == pharmacist.Id // Using pharmacist.Id directly (string)
                                                    && p.Date >= startDate
                                                    && p.Date <= endDate)
                                        .Select(p => new
                                        {
                                            p.Date,
                                            PatientName = $"{p.Patient.Name} {p.Patient.Surname}",
                                            SurgeonName = $"{p.Surgeon.Name} {p.Surgeon.Surname}",
                                            p.PrescriptionStatus,
                                            Medications = p.PrescriptionMedications
                                                .Select(pm => new
                                                {
                                                    pm.MedicationRecords.MedicationName,
                                                    pm.Quantity,
                                                    pm.Instructions
                                                }).ToList()
                                        })
                                        .ToList();

            // Calculate medication summary
            var medicationSummary = _context.PrescriptionMedications
                .Where(pm => pm.PrescriptionRecords.PharmacistID == pharmacist.Id // Using pharmacist.Id directly (string)
                             && pm.PrescriptionRecords.Date >= startDate
                             && pm.PrescriptionRecords.Date <= endDate)
                .GroupBy(pm => pm.MedicationRecords.MedicationName)
                .Select(g => new
                {
                    MedicationName = g.Key,
                    TotalPrescriptions = g.Count()
                })
                .ToList();

            // Pass the required data to the view
            ViewBag.PharmacistName = $"{pharmacist.Name} {pharmacist.Surname}";
            ViewBag.Prescriptions = prescriptions;
            ViewBag.MedicationSummaries = medicationSummary;
            ViewBag.StartDate = startDate;
            ViewBag.EndDate = endDate;

            return View();
        }


        //Rejection Process
        [HttpPost]
        public IActionResult RejectPrescription(int prescriptionID, string rejectionReason)
        {
            var prescription = _context.PrescriptionRecords
                .Include(p => p.Surgeon)
                .Include(p => p.Patient)
                .Include(p => p.PrescriptionMedications)
                .FirstOrDefault(p => p.PrescriptionID == prescriptionID);

            if (prescription == null)
            {
                TempData["error"] = "Prescription not found.";
                return RedirectToAction("Index");
            }

            foreach (var med in prescription.PrescriptionMedications)
            {
                var rejectedPrescription = new RejectedPrescription
                {
                    PrescriptionID = prescription.PrescriptionID,
                    SurgeonID = prescription.SurgeonID,
                    PatientID = prescription.PatientID,
                    Date = prescription.Date,
                    PrescriptionStatus = "Rejected",
                    PharmacistID = prescription.PharmacistID,
                    RejectionReason = rejectionReason,
                    RejectedOn = DateTime.Now,
                    MedicationID = med.MedicationID,
                    Quantity = med.Quantity,
                    Instructions = med.Instructions
                };

                _context.RejectedPrescriptions.Add(rejectedPrescription);
            }

            _context.PrescriptionRecords.Remove(prescription);
            _context.SaveChanges();

            TempData["rejectionPass"] = "Prescription rejected successfully.";


            return RedirectToAction("Index");
        }


        //Dispense Action
        [HttpPost]
        public IActionResult Dispense(int prescriptionID)
        {
            var prescription = _context.PrescriptionRecords
                .Include(p => p.Surgeon)
                .Include(p => p.Patient)
                .Include(p => p.PrescriptionMedications)
                    .ThenInclude(pm => pm.MedicationRecords)
                        .ThenInclude(mr => mr.MedicationActiveIngredients)
                            .ThenInclude(mai => mai.ActiveIngredientRecords)
                .FirstOrDefault(p => p.PrescriptionID == prescriptionID);

            if (prescription == null)
            {
                TempData["error"] = "Prescription not found.";
                return RedirectToAction("Index");
            }

            var patientID = prescription.PatientID;
            var medicalHistory = _context.MedicalHistories
                .Include(mh => mh.MedicalHistoryAllergies)
                    .ThenInclude(mha => mha.ActiveIngredientRecords)
                .Include(mh => mh.MedicalHistoryMedications)
                    .ThenInclude(mhm => mhm.MedicationRecords)
                .Include(mh => mh.MedicalHistoryCondition)
                    .ThenInclude(mhc => mhc.Condition)
                .Where(mh => mh.PatientID == patientID)
                .OrderByDescending(mh => mh.DateRecorded)
                .FirstOrDefault();

            var criticalAlerts = new List<string>();
            var nonCriticalAlerts = new List<string>();

            if (medicalHistory != null)
            {
                // Check allergies
                foreach (var med in prescription.PrescriptionMedications)
                {
                    var medication = med.MedicationRecords;
                    if (medication != null && medication.MedicationActiveIngredients != null)
                    {
                        foreach (var allergy in medicalHistory.MedicalHistoryAllergies)
                        {
                            if (medication.MedicationActiveIngredients.Any(mai =>
                                mai.ActiveIngredientRecords != null &&
                                mai.ActiveIngredientRecords.IngredientID == allergy.ActiveIngredientRecords.IngredientID))
                            {
                                criticalAlerts.Add($"Allergy warning: Patient is allergic to {allergy.ActiveIngredientRecords.IngredientName}, which is present in {medication.MedicationName}.");
                            }
                        }
                    }
                }

                var medicationInteractions = GetMedicationInteractions();

                // Check current medications and prescribed medications for interactions
                foreach (var prescribedMed in prescription.PrescriptionMedications)
                {
                    // Check against current medications
                    foreach (var currentMed in medicalHistory.MedicalHistoryMedications)
                    {
                        if (HasInteraction(prescribedMed.MedicationRecords.MedicationName, currentMed.MedicationRecords.MedicationName, medicationInteractions))
                        {
                            criticalAlerts.Add($"Interaction warning: {prescribedMed.MedicationRecords.MedicationName} may interact with {currentMed.MedicationRecords.MedicationName} which the patient is currently taking.");
                        }
                    }

                    // Check against other prescribed medications
                    foreach (var otherPrescribedMed in prescription.PrescriptionMedications)
                    {
                        if (prescribedMed != otherPrescribedMed &&
                            HasInteraction(prescribedMed.MedicationRecords.MedicationName, otherPrescribedMed.MedicationRecords.MedicationName, medicationInteractions))
                        {
                            criticalAlerts.Add($"Interaction warning: {prescribedMed.MedicationRecords.MedicationName} may interact with {otherPrescribedMed.MedicationRecords.MedicationName} which is also being prescribed.");
                        }
                    }

                    // Still notify if the medication is already being taken
                    var medicationMatch = medicalHistory.MedicalHistoryMedications
                        .FirstOrDefault(mhm => mhm.MedicationRecords.MedicationName.ToLower() == prescribedMed.MedicationRecords.MedicationName.ToLower());
                    if (medicationMatch != null)
                    {
                        nonCriticalAlerts.Add($"Note: Patient is already taking {prescribedMed.MedicationRecords.MedicationName}");
                    }
                }

                // Check conditions
                if (medicalHistory.MedicalHistoryCondition.Any())
                {
                    var conditions = string.Join(", ", medicalHistory.MedicalHistoryCondition.Select(mhc => mhc.Condition.Diagnosis));
                    nonCriticalAlerts.Add($"Patient conditions: {conditions}");
                }
            }

            if (criticalAlerts.Any())
            {
                TempData["criticalAlerts"] = criticalAlerts.ToArray();
                TempData["nonCriticalAlerts"] = nonCriticalAlerts.ToArray();
                TempData["highlightPrescriptionID"] = prescriptionID;
                return RedirectToAction("Index");
            }

            if (nonCriticalAlerts.Any())
            {
                TempData["nonCriticalAlerts"] = nonCriticalAlerts.ToArray();
                TempData["highlightPrescriptionID"] = prescriptionID;
            }

            // Existing dispensing logic
            foreach (var med in prescription.PrescriptionMedications)
            {
                var medication = med.MedicationRecords;
                if (medication != null)
                {
                    if (int.TryParse(medication.QuantityOnHand, out int currentQuantity))
                    {
                        if (currentQuantity < med.Quantity)
                        {
                            TempData["error"] = $"You cannot dispense {medication.MedicationName}. Quantity on hand is {medication.QuantityOnHand}, please re-stock Medication.";
                            TempData["highlightMedicationID"] = medication.MedicationID;
                            return RedirectToAction("Index");
                        }

                        var dispensedPrescription = new DispensedPrescription
                        {
                            PrescriptionID = prescription.PrescriptionID,
                            SurgeonID = prescription.SurgeonID,
                            PatientID = prescription.PatientID,
                            Date = DateTime.Now,
                            PrescriptionStatus = "Dispensed",
                            PharmacistID = prescription.PharmacistID,
                            MedicationID = med.MedicationID,
                            Quantity = med.Quantity,
                            Instructions = med.Instructions
                        };
                        _context.DispensedPrescriptions.Add(dispensedPrescription);

                        medication.QuantityOnHand = (currentQuantity - med.Quantity).ToString();
                    }
                }
            }
            prescription.PrescriptionStatus = "Dispensed";
            _context.PrescriptionRecords.Update(prescription);
            _context.SaveChanges();
            TempData["dispensePass"] = "Prescription successfully dispensed and Stock on Hand updated.";
            return RedirectToAction("Index");
        }

        private List<(string, string)> GetMedicationInteractions()
        {
            // This could be loaded from a database or a configuration file
            return new List<(string, string)>
    {
        ("Compral", "Aspirin"),
        ("Warfarin", "Aspirin"),
        ("Carbimazole","Doxazosin"),
        ("Doxazosin","Doxylamine Succinate " ),
        // Add more interactions as needed
    };
        }

        private bool HasInteraction(string med1, string med2, List<(string, string)> interactions)
        {
            return interactions.Any(i =>
                (i.Item1.ToLower() == med1.ToLower() && i.Item2.ToLower() == med2.ToLower()) ||
                (i.Item1.ToLower() == med2.ToLower() && i.Item2.ToLower() == med1.ToLower()));
        }












        // GET: PrescriptionRecords/Index
        public async Task<IActionResult> Index(string patientId)
        {
            // Get the logged-in user's email address
            var userEmail = User.Identity.Name;

            // Check if the user is a Pharmacist
            var pharmacist = await _context.MedicalProfessionalRecords
                                           .FirstOrDefaultAsync(p => p.EmailAddress == userEmail && p.Specialization == SpecializationType.Pharmacist);

            if (pharmacist != null)
            {
                ViewBag.IsPharmacist = true;
            }

            // Check if the user is a Surgeon
            var surgeon = await _context.MedicalProfessionalRecords
                                           .FirstOrDefaultAsync(p => p.EmailAddress == userEmail && p.Specialization == SpecializationType.Surgeon);

            if (surgeon != null)
            {
                ViewBag.IsSurgeon = true;
            }

            // Retrieve prescription records
            IQueryable<PrescriptionRecords> query = _context.PrescriptionRecords
                .Include(p => p.PrescriptionMedications)
                .ThenInclude(pm => pm.MedicationRecords)
                .Include(p => p.Patient)
                .Include(p => p.Surgeon).Where(c=>c.PrescriptionStatus == "Normal" || c.PrescriptionStatus == "Urgent");

            // Adjust query based on role (Pharmacist or Surgeon)
            if (pharmacist != null)
            {
                // If the user is a pharmacist, filter by PharmacistID
                query = query.Where(p => p.PharmacistID == pharmacist.Id);
            }
            else if (surgeon != null)
            {
                // If the user is a surgeon, filter by SurgeonID
                query = query.Where(p => p.SurgeonID == surgeon.Id);
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

        // POST: PrescriptionRecords/SearchPrescription
        [HttpPost]
        public IActionResult SearchPrescription(string patientId)
        {
            return RedirectToAction("Index", new { patientId });
        }



        // GET: PrescriptionRecords/Details/5
        public IActionResult Create()
        {
            try
            {
                var userName = User.Identity.Name;

                // Find the corresponding surgeon based on the logged-in user's email
                var surgeon = _context.MedicalProfessionalRecords
                    .Where(m => m.Specialization == SpecializationType.Surgeon && m.EmailAddress == userName)
                    .Select(m => new { m.Id, FullName = m.Name + " " + m.Surname })
                    .FirstOrDefault();

                // Get list of pharmacists
                var pharmacists = _context.MedicalProfessionalRecords
                    .Where(m => m.Specialization == SpecializationType.Pharmacist)
                    .Select(m => new { m.Id, FullName = m.Name + " " + m.Surname })
                    .ToList();

                // Populate the dropdowns with the required data
                var medications = _context.MedicationRecords.ToList(); // Assuming _context is your DbContext
                ViewBag.MedicationList = medications;
                ViewData["PatientID"] = new SelectList(_context.Patients
                    .Select(p => new { p.PatientID, FullName = p.Name + " " + p.Surname })
                    .ToList(), "PatientID", "FullName");

                // Fixing PharmacistID to use the correct Id field (string)
                ViewData["PharmacistID"] = new SelectList(pharmacists, "Id", "FullName");

                // Set PrescriptionStatus dropdown
                ViewData["PrescriptionStatus"] = new SelectList(new List<SelectListItem>
        {
            new SelectListItem { Value = "Urgent", Text = "Urgent" },
            new SelectListItem { Value = "Normal", Text = "Normal" },
            // Add other statuses here
        }, "Value", "Text");

                // Set the surgeon dropdown, pre-selecting the logged-in Surgeon if found
                ViewData["SurgeonID"] = surgeon != null
                    ? new SelectList(new List<dynamic> { surgeon }, "Id", "FullName", surgeon.Id)
                    : new SelectList(Enumerable.Empty<dynamic>());

                return View();
            }
            catch (Exception ex)
            {
                // Handle the exception as needed (log it, display an error message, etc.)
                return BadRequest("An error occurred while loading the Create view.");
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> Create(
    [Bind("PrescriptionID,SurgeonID,PatientID,PrescriptionDate,PrescriptionStatus,PharmacistID")] PrescriptionRecords prescription,
    string medicationIds, string quantities, string instructions)
        {
            var medicationIdList = medicationIds?.Split(',').Select(int.Parse).ToList();
            var quantityList = quantities?.Split(',').Select(int.Parse).ToList();
            var instructionList = instructions?.Split(',').ToList();

            if (medicationIdList == null || quantityList == null || instructionList == null ||
                medicationIdList.Count != quantityList.Count || quantityList.Count != instructionList.Count)
            {
                // Handle mismatched or incomplete data
                return Json(new { success = false, message = "Medication details are incomplete or mismatched." });
            }

            try
            {
                // Save the PrescriptionRecord
                _context.PrescriptionRecords.Add(prescription);
                await _context.SaveChangesAsync();

                // Save the PrescriptionMedications
                for (int i = 0; i < medicationIdList.Count; i++)
                {
                    var prescriptionMedication = new Health_Hive_Project_2024.Models.PrescriptionMedications
                    {
                        PrescriptionID = prescription.PrescriptionID,
                        MedicationID = medicationIdList[i],
                        Quantity = quantityList[i],
                        Instructions = instructionList[i]
                    };
                    _context.PrescriptionMedications.Add(prescriptionMedication);
                }
                await _context.SaveChangesAsync();

                // Return success response
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                // Log the error and return a failure response
                // (Logging is optional, depending on your setup)
                // _logger.LogError(ex, "An error occurred while saving prescription.");

                return Json(new { success = false, message = "An error occurred while saving the prescription." });
            }
        }



        private void PopulateDropdowns()
        {
            ViewBag.SurgeonID = new SelectList(_context.MedicalProfessionalRecords
                .Where(m => m.Specialization == SpecializationType.Surgeon)
                .Select(m => new { m.Id, FullName = m.Name + " " + m.Surname }), "Id", "FullName");

            ViewBag.PharmacistID = new SelectList(_context.MedicalProfessionalRecords
                .Where(m => m.Specialization == SpecializationType.Pharmacist)
                .Select(m => new { m.Id, FullName = m.Name + " " + m.Surname }), "Id", "FullName");

            ViewBag.PatientID = new SelectList(_context.Patients
                .Select(p => new { p.PatientID, FullName = p.Name + " " + p.Surname }), "PatientID", "FullName");
        }












        // GET: PrescriptionRecords/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var prescriptionRecords = await _context.PrescriptionRecords.FindAsync(id);
            if (prescriptionRecords == null)
            {
                return NotFound();
            }

            var surgeons = _context.MedicalProfessionalRecords
               .Where(m => m.Specialization == SpecializationType.Surgeon)
               .Select(m => new { m.Id, FullName = m.Name + " " + m.Surname })
               .ToList(); // Ensure it's a list

            var pharmacists = _context.MedicalProfessionalRecords
                .Where(m => m.Specialization == SpecializationType.Pharmacist)
                .Select(m => new { m.Id, FullName = m.Name + " " + m.Surname })
                .ToList(); // Ensure it's a list


            //ViewData["MedicationID"] = new SelectList(_context.MedicationRecords.ToList(), "MedicationID", "MedicationName", prescriptionRecords.MedicationID);
            ViewData["PatientID"] = new SelectList(_context.Patients.Select(p => new { p.PatientID, FullName = p.Name + " " + p.Surname }).ToList(), "PatientID", "FullName", prescriptionRecords.PatientID);
            ViewData["PharmacistID"] = new SelectList(pharmacists, "Id", "FullName", prescriptionRecords.PharmacistID);
            ViewData["SurgeonID"] = new SelectList(surgeons, "Id", "FullName", prescriptionRecords.SurgeonID);
            return View(prescriptionRecords);
        }

        // POST: PrescriptionRecords/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PrescriptionID,SurgeonID,PatientID,Date,PrescriptionStatus,PharmacistID,MedicationID,Quantity,Instructions")] PrescriptionRecords prescriptionRecords)
        {
            if (id != prescriptionRecords.PrescriptionID)
            {
                return NotFound();
            }


            try
            {
                _context.Update(prescriptionRecords);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PrescriptionRecordsExists(prescriptionRecords.PrescriptionID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));

            var surgeons = _context.MedicalProfessionalRecords
               .Where(m => m.Specialization == SpecializationType.Surgeon)
               .Select(m => new { m.Id, FullName = m.Name + " " + m.Surname })
               .ToList(); // Ensure it's a list

            var pharmacists = _context.MedicalProfessionalRecords
                .Where(m => m.Specialization == SpecializationType.Pharmacist)
                .Select(m => new { m.Id, FullName = m.Name + " " + m.Surname })
                .ToList(); // Ensure it's a list


            //ViewData["MedicationID"] = new SelectList(_context.MedicationRecords.ToList(), "MedicationID", "MedicationName", prescriptionRecords.MedicationID);
            ViewData["PatientID"] = new SelectList(_context.Patients.Select(p => new { p.PatientID, FullName = p.Name + " " + p.Surname }).ToList(), "PatientID", "FullName", prescriptionRecords.PatientID);
            ViewData["PharmacistID"] = new SelectList(pharmacists, "Id", "FullName", prescriptionRecords.PharmacistID);
            ViewData["SurgeonID"] = new SelectList(surgeons, "Id", "FullName", prescriptionRecords.SurgeonID);
            return View(prescriptionRecords);
        }



        //Change code

        // GET: PrescriptionRecords/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var prescriptionRecords = await _context.PrescriptionRecords
        //        .Include(p => p.Medication)
        //        .Include(p => p.Patient)
        //        .Include(p => p.Pharmacist)
        //        .Include(p => p.Surgeon)
        //        .FirstOrDefaultAsync(m => m.PrescriptionID == id);
        //    if (prescriptionRecords == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(prescriptionRecords);
        //}

        // POST: PrescriptionRecords/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var prescriptionRecords = await _context.PrescriptionRecords.FindAsync(id);
            if (prescriptionRecords != null)
            {
                _context.PrescriptionRecords.Remove(prescriptionRecords);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PrescriptionRecordsExists(int id)
        {
            return _context.PrescriptionRecords.Any(e => e.PrescriptionID == id);
        }



        // API endpoint for searching medications
        [HttpGet]
        [Route("PrescriptionRecords/SearchMedications")]
        public async Task<IActionResult> SearchMedications([FromQuery] string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return BadRequest("Search query cannot be empty.");
            }

            var medications = await _context.MedicationRecords
                .Where(m => m.MedicationName.Contains(query))
                .Select(m => new
                {
                    m.MedicationID,
                    m.MedicationName
                })
                .ToListAsync();

            return Ok(medications);
        }







        // API endpoint to get details of a specific medication by MedicationID
        [HttpGet]
        [Route("PrescriptionRecords/GetMedication/{id}")]
        public async Task<IActionResult> GetMedication(int id)
        {
            var medication = await _context.MedicationRecords
                .Include(m => m.DosageForm)
                .Include(m => m.MedicationActiveIngredients) // if you want to include related data
                .FirstOrDefaultAsync(m => m.MedicationID == id);

            if (medication == null)
            {
                return NotFound("Medication not found.");
            }

            return Ok(medication);
        }






    }
}

