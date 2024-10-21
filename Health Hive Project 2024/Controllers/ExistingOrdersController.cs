using Health_Hive_Project_2024.Data;
using Health_Hive_Project_2024.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Health_Hive_Project_2024.Controllers
{
    public class ExistingOrdersController : Controller
    {

        private readonly ApplicationDbContext _context;

        public ExistingOrdersController(ApplicationDbContext context)
        {
            _context = context;
        }

        //[HttpGet]
        //public async Task<IActionResult> Index(string searchId)
        //{
        //    // Store the searchId in ViewData for retaining the search input in the view
        //    ViewData["CurrentFilter"] = searchId;

        //    // Query Medication Orders, include Patient and MedicationData
        //    //var query = _context.GetMedicationOrder
        //    //    .Include(a => a.GetMedicationData)
        //    //    .Include(a => a.Patient); // Ensure Patient data is loaded

        //    // Apply the filter if searchId is provided and ensure PatientIDNumber is properly loaded
        //    if (!string.IsNullOrEmpty(searchId))
        //    {
        //        var query = _context.GetMedicationOrder.Include(a => a.GetMedicationData)
        //        .Include(a => a.Patient)
        //        .Where(a => a.Status == Models.OrderStatus.Ordered)
        //        .Where(m => m.Patient.PatientIDNumber.Contains(searchId));
        //    }

        //    // Execute the query and retrieve the results
        //    //var medicationOrders = await query.Where(a => a.Status == Models.OrderStatus.Ordered).ToListAsync();

        //    // Load MedicationRecords and pass them to ViewBag
        //    var medicationRecords = await _context.MedicationRecords.ToListAsync();
        //    ViewBag.MedicationRecords = medicationRecords;

        //    return View(query);
        //}


        [HttpGet]
        public async Task<IActionResult> Index(string? searchId)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ViewData["CurrentFilter"] = searchId;
            var solutions = from b in _context.GetMedicationOrder.Include(a => a.Patient).Include(a => a.GetMedicationData).Where(a => a.Status == Models.OrderStatus.Ordered && a.Id == currentUserId)
            select b;
            if (!String.IsNullOrEmpty(searchId))
            {
                solutions = solutions.Where(b => b.Patient.PatientIDNumber.Contains(searchId))
                                                   .Include(b => b.Patient);
            }

            var medicationRecords = await _context.MedicationRecords.ToListAsync();
                 ViewBag.MedicationRecords = medicationRecords;
            return View(solutions);
        }


        [HttpGet]
        public async Task<IActionResult> GetMedications(int OrderMedicationID)
        {
            var medications = await _context.GetMedicationData
                .Where(mo => mo.OrderMedicationID == OrderMedicationID)
                .Include(m => m.Medication) // Assuming Medication is a navigation property
                .Select(ds => new
                {
                    MedicineStoreId = ds.MedicineStoreId,
                    MedicationName = ds.Medication.MedicationName,
                    Quantity = ds.Quantity,
                    Note = ds.Note
                })
                .ToListAsync();

            return Json(medications);
        }
        [HttpDelete]
        public IActionResult DeleteMedication(int? medicineStoreId)
        {
            if (medicineStoreId == 0)
            {
                return Json(new { message = "Invalid medication ID." });
            }
            var medicationToDelete = _context.GetMedicationData.FirstOrDefault(m => m.MedicineStoreId == medicineStoreId);

            var or = _context.GetMedicationData.Include(a => a.Medication).Include(a => a.GetOrder)
                    .FirstOrDefault(o => o.MedicineStoreId == medicineStoreId);
            if(medicationToDelete == null)
            {
                return Json(new { message = "Medication not found. Please reload to see changes!" });
            }
            var notification = new NortificationModel
            {
                Id = User.FindFirstValue(ClaimTypes.NameIdentifier), // Set the ID of the logged-in user
                Description = $"Medication '{or.Medication.MedicationName}' on order '{or.GetOrder.PrescriptionOrder}' has been successfully deleted!", // Description of the vital
                TimeStamp = DateTime.Now // Set the current timestamp
            };
            

            _context.GetNortifications.Add(notification);
            _context.GetMedicationData.Remove(medicationToDelete);
            _context.SaveChangesAsync();

            return Json(new { message = "Medication deleted successfully. Please reload to see the changes!" });
        }


        [HttpPost]
        public IActionResult AddMedication(MedicationDataStore newMedication)
        {
            if (!ModelState.IsValid)
            {

                newMedication.Note = "";
                // Add new medication to the database
                _context.GetMedicationData.Add(newMedication);
               
               
             
               

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
                return RedirectToAction("Index", "ExistingOrders", newMedication);
            }

          
            // Redirect back to the Information view with the patient ID and show validation errors
            return RedirectToAction("Index", "ExistingOrders");
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
            return RedirectToAction("Index", "ExistingOrders");
        }



    }
}
