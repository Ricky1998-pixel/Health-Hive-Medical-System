using Health_Hive_Project_2024.Data;
using Health_Hive_Project_2024.Data.ViewModels;
using Health_Hive_Project_2024.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Security.Cryptography;

namespace Health_Hive_Project_2024.Controllers
{
    public class OrderMedicationController : Controller
    {

        private readonly ApplicationDbContext _context;
        private readonly UserManager<MedicalProfessionalRecords> _userManager;
        private readonly IEmailService _emailService;
        //private readonly UserManager<IdentityUser> _userManager;
        public OrderMedicationController(ApplicationDbContext context, UserManager<MedicalProfessionalRecords> userManager, IEmailService emailService)
        {
            _context = context;
            _userManager = userManager;
            _userManager = userManager;
            _emailService = emailService;
        }

        public IActionResult newOrder()
        {
            return View();
        }
        //[HttpGet]
        //public async Task<IActionResult> GetMedicationData(int orderId)
        //{
        //    var dataStores = await _context.GetMedicationData
        //        .Where(mo => mo.GetOrder.OrderMedicationID == orderId)
        //        .Include(a => a.GetOrder)
        //        .Include(p => p.Medication)
        //        .Select(ds => new
        //        {
        //            MedicationName = ds.Medication.MedicationName,
        //            Quantity = ds.Quantity,
        //            Note = ds.Note
        //        })
        //        .ToListAsync();

        //    return Json(dataStores);
        //}

        // Controller for loading medications
        [HttpGet]
        public async Task<IActionResult> GetMedications(int OrderMedicationID)
        {
       
            var medications = await _context.GetMedicationData
                .Where(mo => mo.GetOrder.OrderMedicationID == OrderMedicationID)
                .Include(a => a.GetOrder)
                .Include(p => p.Medication)
                .Select(ds => new
                {
                    MedicineStoreId = ds.MedicineStoreId,
                    MedicationName = ds.Medication.MedicationName,
                    Quantity = ds.Quantity,
                    Note = ds.Note
                })
                .ToListAsync();

            // Return the medications as JSON
            return Json(medications);
        }

        [HttpPost]
        public IActionResult AddMedication(MedicationDataStore newMedication)
        {
            if (!ModelState.IsValid)
            {
                newMedication.Note = "";
                // Add new medication to the database
                _context.GetMedicationData.Add(newMedication);
               

                // Retrieve the PatientID from the associated order
                var patientId = _context.GetMedicationOrder
                    .Where(order => order.OrderMedicationID == newMedication.OrderMedicationID).Include(a => a.Patient)
                    .Select(order => order.PatientID)
                    .FirstOrDefault();
                var order = _context.GetMedicationOrder
                          .FirstOrDefault(o => o.OrderMedicationID == newMedication.OrderMedicationID);
                var notification = new NortificationModel
                {
                    Id = User.FindFirstValue(ClaimTypes.NameIdentifier), // Set the ID of the logged-in user
                    Description = $"Medication  on order '{order.PrescriptionOrder}' has been successfully deleted!", // Description of the vital
                    TimeStamp = DateTime.Now // Set the current timestamp
                };
                _context.GetNortifications.Add(notification);
                _context.SaveChanges();
                if (order != null)
                {
                    // Set success message with the specific prescription order
                    TempData["SuccessMessage"] = $"Medication added successfully to order {order.PrescriptionOrder}!";
                }
                else
                {
                    TempData["SuccessMessage"] = "Medication added successfully, but the associated order was not found.";
                }
                // Redirect to the Information view of the selected patient after successful medication addition
                return Json(new
                {
                    success = true,
                    message = $"Medication added successfully to order {order.PrescriptionOrder}!",
                    redirectUrl = Url.Action("Information", "PatientRecord", new { id = patientId })
                });
            }

            // If model validation fails, retrieve the PatientID from the associated order
            var invalidPatientId = _context.GetMedicationOrder
                .Where(order => order.OrderMedicationID == newMedication.OrderMedicationID)
                .Select(order => order.PatientID)
                .FirstOrDefault();
              
              
            // Redirect back to the Information view with the patient ID and show validation errors
            return RedirectToAction("Information", "PatientRecord", new { id = invalidPatientId });
        }



        [HttpPost]
        public IActionResult UpdateMedicationNote([FromBody] UpdateMedicationNoteRequest request)
        {
            var medication = _context.GetMedicationData.Find(request.MedicineStoreId);
            if (medication == null)
            {
                return NotFound();
            }

            // Update the note
            medication.Note = request.Note;
            _context.SaveChanges();

            return Ok(new { message = "Note updated successfully." });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddVitals(PatientVitals newVital)
        {
            // Set the UserId
            

            // Save new vital record to the database
            _context.PatientVitals.Add(newVital);
            await _context.SaveChangesAsync();

            // Retrieve the associated order
            var ord = _context.Vitals.FirstOrDefault(o => o.VitalID == newVital.VitalID);

            if (ord != null)
            {
                // Set success message with the specific prescription order
                TempData["SuccessMessage"] = $"New Vital successfully added for {ord.Name}!";

                // Create a notification model
                var notification = new NortificationModel
                {
                    Id = User.FindFirstValue(ClaimTypes.NameIdentifier), // Set the ID of the logged-in user
                    Description = $"Vital '{ord.VitalName}' has been added with a reading of {newVital.Value1}.", // Description of the vital
                    TimeStamp = DateTime.Now // Set the current timestamp
                };

                // Save the notification to the database
                _context.GetNortifications.Add(notification);
                await _context.SaveChangesAsync(); // Ensure to save changes after adding the notification
            }
            else
            {
                TempData["SuccessMessage"] = "Vital added successfully, but the associated order was not found.";
            }

            // Retrieve the patient details
            var patient = await _context.Patients.FindAsync(newVital.PatientID); // Use FindAsync for better performance with async

            // Get all users in the "Nurse" role using UserManager
            var nurses = await _userManager.GetUsersInRoleAsync("Nurse");

            // Loop through nurses and send an email to each one
            foreach (var nurse in nurses)
            {
                var emailBody = $@"
Dear {nurse.Name} {nurse.Surname},

A new vital reading has been added for the following patient:

Patient Name: {patient.Name} {patient.Surname}
Vital Reading: {ord.VitalName} reading value is {newVital.Value1}
Date: {newVital.DateTime.ToString("f")}

Please review the patient's record in the system at your earliest convenience.

Best regards,
Health Hive Team";

                // Send an email to each nurse
                await _emailService.SendEmailAsync(nurse.Email, "New Vital Reading Added", emailBody);
            }


            // Redirect back to the PatientRecord page after saving the data
            return RedirectToAction("Information", "PatientRecord", new { id = newVital.PatientID });
        }







        public async Task<IActionResult> LoadNewOrderForm(int patientId)
        {
            var newOrder = new OrderMedications
            {
                PatientID = patientId
            };
            // Retrieve the list of pharmacists with the nurse role
            var pharmacists = await _userManager.GetUsersInRoleAsync("Pharmacist");

            ViewBag.Pharmacists = pharmacists; // Pass the list to the view
            return PartialView("_NewOrderForm", newOrder);
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
            return RedirectToAction("Information", "PatientRecord", new { id = PatientID });
        }
        [HttpPost]
        public async Task<IActionResult> CreateOrder(OrderMedications newOrder)
        {
            // Generate a unique PrescriptionOrder (you could make this more complex if needed)
            newOrder.PrescriptionOrder = "RX-" + Guid.NewGuid().ToString().Substring(0, 8).ToUpper();
            //var currentUser = await _userManager.GetUserAsync(User);
            // The logged-in user's ID (use User.Identity.Name or any other way to retrieve the ID)
            newOrder.Id = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // PharmacistID is left null for now, it will be updated later by the pharmacist
           

            if (!ModelState.IsValid)
            {

                _context.GetMedicationOrder.Add(newOrder);
                _context.SaveChanges();
                TempData["SuccessMessage"] = $"New Order has been successfully added {newOrder.PrescriptionOrder}!";
                var notification = new NortificationModel
                {
                    Id = User.FindFirstValue(ClaimTypes.NameIdentifier), // Set the ID of the logged-in user
                    Description = $"Order '{newOrder.PrescriptionOrder}' has been successfully added!", // Description of the vital
                    TimeStamp = DateTime.Now // Set the current timestamp
                };
                _context.GetNortifications.Add(notification);
                await _context.SaveChangesAsync();
                // Redirect to the next step, which adds medications to the order
                return RedirectToAction("Information", "PatientRecord", new { id = newOrder.PatientID });
            }

            // If there's a validation error, redisplay the form
            return PartialView("_NewOrderForm", newOrder);
        }


        [HttpPost]
        public IActionResult EditOrder(int OrderMedicationID, bool IsUrgent, string Instructions, int PatientID)
        {
            var order = _context.GetMedicationOrder.Find(OrderMedicationID);
            if (order != null)
            {
                order.IsUrgent = IsUrgent;
                order.Instructions = Instructions;
                _context.SaveChanges();
                var ord = _context.GetMedicationOrder
                       .FirstOrDefault(o => o.OrderMedicationID == OrderMedicationID);
                var notification = new NortificationModel
                {
                    Id = User.FindFirstValue(ClaimTypes.NameIdentifier), // Set the ID of the logged-in user
                    Description = $"Order '{ord.PrescriptionOrder}' has been successfully Edited!", // Description of the vital
                    TimeStamp = DateTime.Now // Set the current timestamp
                };
                _context.GetNortifications.Add(notification);
                _context.SaveChangesAsync();
                if (ord != null)
                {
                    // Set success message with the specific prescription order
                    TempData["SuccessMessage"] = $"Medication Order successfully editted on {ord.PrescriptionOrder}!";

                }
                else
                {
                    TempData["SuccessMessage"] = "Medication added successfully, but the associated order was not found.";
                }
            }

            // Redirect back to the same patient's details or order view
            return RedirectToAction("Information", "PatientRecord", new { id = PatientID });
        }

        //Dashboard OrderMedication Process

        public IActionResult Process()
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

                var patients = _context.Patients
           .Select(p => new { p.PatientID, FullName = p.Name + " " + p.Surname })
           .ToList();

                // Populate the dropdowns with the required data
                var medications = _context.MedicationRecords.ToList(); // Assuming _context is your DbContext
                ViewBag.MedicationList = medications;
                ViewData["PatientID"] = new SelectList(_context.Patients
                    .Select(p => new { p.PatientID, FullName = p.Name + " " + p.Surname })
                    .ToList(), "PatientID", "FullName");

                ViewBag.PatientId = new SelectList(patients, "PatientID", "FullName");
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
    OrderMedications prescription,
    string medicationIds, string quantities, string instructions)
        {
            var medicationIdList = medicationIds?.Split(',').Select(int.Parse).ToList();
            var quantityList = quantities?.Split(',').Select(int.Parse).ToList();
            var instructionList = instructions?.Split(',').ToList();
            prescription.PrescriptionOrder = "RX-" + Guid.NewGuid().ToString().Substring(0, 8).ToUpper();
            if (medicationIdList == null || quantityList == null || instructionList == null ||
                medicationIdList.Count != quantityList.Count || quantityList.Count != instructionList.Count)
            {
                // Handle mismatched or incomplete data
                return Json(new { success = false, message = "Medication details are incomplete or mismatched." });
            }

            try
            {
                prescription.Date = DateTime.Now;
                // Save the PrescriptionRecord
                prescription.Id = User.FindFirstValue(ClaimTypes.NameIdentifier);
                _context.GetMedicationOrder.Add(prescription);
                await _context.SaveChangesAsync();
                var notification = new NortificationModel
                {
                    Id = User.FindFirstValue(ClaimTypes.NameIdentifier), // Set the ID of the logged-in user
                    Description = $"Order '{prescription.PrescriptionOrder}' has been successfully added!", // Description of the vital
                    TimeStamp = DateTime.Now // Set the current timestamp
                };
                _context.GetNortifications.Add(notification);
                await _context.SaveChangesAsync();
                // Save the PrescriptionMedications
                for (int i = 0; i < medicationIdList.Count; i++)
                {
                    var prescriptionMedication = new MedicationDataStore
                    {
                        OrderMedicationID = prescription.OrderMedicationID,
                        MedicationID = medicationIdList[i],
                        Quantity = quantityList[i],
                        Note = instructionList[i]
                    };
                    _context.GetMedicationData.Add(prescriptionMedication);
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
     //   [HttpGet]
     //   public async Task<IActionResult> SearchPatient(string patientIdNumber)
     //   {
     //       if (string.IsNullOrEmpty(patientIdNumber))
     //       {
     //           return PartialView("_Details", null);
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

     //       return PartialView("_Details", patient);
     //   }


        [HttpPost]
        public async Task<IActionResult> Order(OrderMedications model)
        {
            model.PrescriptionOrder = "RX-" + Guid.NewGuid().ToString().Substring(0, 8).ToUpper();


            model.Id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            model.PharmacistID = "";
            if (!ModelState.IsValid)
            {
                _context.GetMedicationOrder.Add(model);
                await _context.SaveChangesAsync();
                return RedirectToAction( "Index", "ExistingOrders"); // Redirect or display success message
            }


            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> FilterVitals(string vitalName, int id)
        {
            var filteredVitals = await _context.GetVitalModels.Include(a => a.GetNormalRange)
                .Where(v => v.PatientID == id && (string.IsNullOrEmpty(vitalName) || v.GetNormalRange.VitalName == vitalName))
                .OrderByDescending(v => v.TimeStamp)
                .ToListAsync();

            return PartialView("_FilteredVitals", filteredVitals);
        }





        [HttpDelete]
        public IActionResult DeleteMedication(int? medicineStoreId)
        {
            if (medicineStoreId == 0)
            {
                return Json(new { message = "Invalid medication ID." });
            }

            var medicationToDelete = _context.GetMedicationData.FirstOrDefault(m => m.MedicineStoreId == medicineStoreId);

            if (medicationToDelete == null)
            {
                return Json(new { message = "Medication not found. Please reload to see changes!" });
            }

            _context.GetMedicationData.Remove(medicationToDelete);
            _context.SaveChanges();
            var or = _context.GetMedicationData.Include(a => a.Medication).Include(a => a.GetOrder)
                       .FirstOrDefault(o => o.MedicineStoreId == medicineStoreId);
            var notification = new NortificationModel
            {
                Id = User.FindFirstValue(ClaimTypes.NameIdentifier), // Set the ID of the logged-in user
                Description = $"Medication '{or.Medication.MedicationName}' on order '{or.GetOrder.PrescriptionOrder}' has been successfully added!", // Description of the vital
                TimeStamp = DateTime.Now // Set the current timestamp
            };
            _context.GetNortifications.Add(notification);
             _context.SaveChangesAsync();
            return Json(new { message = "Medication deleted successfully. Please reload to see the changes!" });
        }

    }
    // DTO for handling the request
    public class UpdateMedicationNoteRequest
    {
        public int MedicineStoreId { get; set; }
        public string Note { get; set; }
    }
}
