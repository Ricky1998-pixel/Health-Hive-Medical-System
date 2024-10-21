using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Health_Hive_Project_2024.Data;
using Health_Hive_Project_2024.Models;
using System.Net.Mail;
using System.Net;
using System.Text;

namespace Health_Hive_Project_2024.Controllers
{
    public class MedicationRecordsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmailService _emailService;

        public MedicationRecordsController(ApplicationDbContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService; // Injected Email Service
        }


        //Reports
        public IActionResult Reports()
        {
            var dispensedPrescriptions = _context.DispensedPrescriptions.ToList();
            var rejectedPrescriptions = _context.RejectedPrescriptions.ToList();
            var medicationRecords = _context.MedicationRecords.ToList();

            ViewData["DispensedPrescriptions"] = dispensedPrescriptions;
            ViewData["RejectedPrescriptions"] = rejectedPrescriptions;
            ViewData["MedicationRecords"] = medicationRecords;

            return View();
        }

        //Bulk Stock Capure
        [HttpPost]
        public IActionResult CaptureStock(Dictionary<int, int> stockQuantities)
        {
            foreach (var medicationId in stockQuantities.Keys)
            {
                var medication = _context.MedicationRecords.Find(medicationId);
                if (medication != null)
                {
                    int capturedQuantity = stockQuantities[medicationId];
                    // Update the quantity on hand
                    int currentQuantity = int.TryParse(medication.QuantityOnHand, out int result) ? result : 0;
                    medication.QuantityOnHand = (currentQuantity + capturedQuantity).ToString();
                    _context.SaveChanges();
                }
            }

            return RedirectToAction("Index");
        }








        // BULK ACTION FOR ODERING
        [HttpPost]
        public async Task<IActionResult> BulkAction(IFormCollection formCollection)
        {
            var quantities = formCollection
                .Where(x => x.Key.StartsWith("quantities["))
                .ToDictionary(
                    x => x.Key,
                    x => x.Value
                );

            var urgencies = formCollection
                .Where(x => x.Key.StartsWith("urgent["))
                .ToDictionary(
                    x => x.Key,
                    x => x.Value
                );

            if (quantities == null || !quantities.Any())
            {
                return RedirectToAction("Index"); // Redirect if no items are selected
            }

            foreach (var entry in quantities)
            {
                int medicationId = int.Parse(entry.Key.Replace("quantities[", "").Replace("]", ""));
                int quantity;

                if (int.TryParse(entry.Value, out quantity) && quantity > 0)
                {
                    var existingOrderItem = _context.OrderItem
                        .FirstOrDefault(o => o.MedicationID == medicationId);

                    if (existingOrderItem != null)
                    {
                        existingOrderItem.Quantity += quantity; // Increment the quantity
                        existingOrderItem.IsUrgent = urgencies.ContainsKey($"urgent[{medicationId}]") && urgencies[$"urgent[{medicationId}]"] == "true";
                    }
                    else
                    {
                        var newOrderItem = new OrderItem
                        {
                            MedicationID = medicationId,
                            Quantity = quantity,
                            IsUrgent = urgencies.ContainsKey($"urgent[{medicationId}]") && urgencies[$"urgent[{medicationId}]"] == "true"
                        };

                        _context.OrderItem.Add(newOrderItem);
                    }
                }
            }

            await _context.SaveChangesAsync();

            // Fetch the purchase manager's email address
            var dayHospital = await _context.DayHospitals.FirstOrDefaultAsync(); // Adjust this query as necessary
            if (dayHospital != null)
            {
                string purchaseManagerEmail = dayHospital.PurchaseManagerEmailAddress;
                string subject = "Medication Order Summary-GROUP 16";
                string body = GenerateOrderSummaryEmailBody(); // Create a method to generate the summary email body

                // Send the email
                await SendEmailAsync(purchaseManagerEmail, subject, body);
            }

            return RedirectToAction("Index", new { success = true });
        }

        //EMAIL ASYNC FOR HEALTH HIVE GMAIL.COM
        private async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            var smtpHost = "smtp.gmail.com"; // This is the SMTP host from EmailSettings
            int smtpPort = 587;              // This is the SMTP port from EmailSettings
            string fromAddress = "hivehealth628@gmail.com"; // FromAddress from EmailSettings
            string smtpUsername = "hivehealth628@gmail.com"; // SMTPUsername from EmailSettings
            string smtpPassword = "pcbbjzzjndrmarqi"; // SMTPPassword from EmailSettings

            var mailMessage = new MailMessage(fromAddress, toEmail)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true // Set to true if the body contains HTML
            };

            using (var smtpClient = new SmtpClient(smtpHost, smtpPort))
            {
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential(smtpUsername, smtpPassword);
                smtpClient.EnableSsl = true;

                await smtpClient.SendMailAsync(mailMessage);
            }
        }

        //GENERATING EMAIL FOR ORDERS
        private string GenerateOrderSummaryEmailBody()
        {
            // Fetch order items along with medication details
            var orderItems = (from oi in _context.OrderItem
                              join m in _context.MedicationRecords on oi.MedicationID equals m.MedicationID
                              select new
                              {
                                  MedicationName = m.MedicationName,
                                  oi.Quantity,
                                  oi.IsUrgent
                              }).ToList();

            var sb = new StringBuilder();
            sb.AppendLine("<h1>Medication Order Summary</h1>");
            sb.AppendLine("<table border='1'><tr><th>Medication Name</th><th>Quantity</th><th>Urgent</th></tr>");
            foreach (var item in orderItems)
            {
                sb.AppendLine($"<tr><td>{item.MedicationName}</td><td>{item.Quantity}</td><td>{item.IsUrgent}</td></tr>");
            }
            sb.AppendLine("</table>");
            return sb.ToString();
        }



        // GET: MedicationRecords INDEX
        public async Task<IActionResult> Index()
        {
            var medicationsWithIngredients = await _context.MedicationRecords
                .Include(m => m.DosageForm)
                .Include(m => m.MedicationActiveIngredients)
                    .ThenInclude(ma => ma.ActiveIngredientRecords)
                .ToListAsync();

            return View(medicationsWithIngredients);
        }




        // GET: MedicationRecords/Details/5
        //public async Task<IActionResult> Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var medicationRecords = await _context.MedicationRecords
        //        .Include(m => m.DosageForm)
        //        .FirstOrDefaultAsync(m => m.MedicationID == id);
        //    if (medicationRecords == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(medicationRecords);
        //}

        // GET: MedicationRecords/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medicationRecords = await _context.MedicationRecords
                .Include(m => m.DosageForm)
                .FirstOrDefaultAsync(m => m.MedicationID == id);
            if (medicationRecords == null)
            {
                return NotFound();
            }

            return PartialView("_MedicationDetailsPartial", medicationRecords);
        }




        //EndPoint for fetching data
        [HttpGet]
        public IActionResult GetMedications()
        {
            var medications = _context.MedicationRecords
                .Select(m => new { m.MedicationID, m.MedicationName })
                .ToList();
            return Json(medications);
        }

        // GET: MedicationRecords/Create
        public IActionResult Create()
        {
            // Fetch and sort the list of active ingredients alphabetically by IngredientName
            var activeIngredients = _context.ActiveIngredientRecords
                                            .OrderBy(ingredient => ingredient.IngredientName)
                                            .ToList();

            // Pass the sorted list to the view
            ViewBag.ActiveIngredients = activeIngredients;

            // Pass DosageForm data for dropdown
            ViewData["DosageFormID"] = new SelectList(_context.DosageForm, "DosageFormID", "Form");

            return View();
        }



        // POST: MedicationRecords/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MedicationID,MedicationName,DosageFormID,Schedule,QuantityOnHand,ReOrderLevel")] MedicationRecords medicationRecords, string SelectedIngredients)
        {
            
                _context.Add(medicationRecords);
                await _context.SaveChangesAsync();

                // Save selected active ingredients to MedicationActiveIngredient table
                var ingredientIds = SelectedIngredients.Split(',').Select(int.Parse).ToList();

                foreach (var ingredientId in ingredientIds)
                {
                    var medicationIngredient = new MedicationActiveIngredient
                    {
                        MedicationID = medicationRecords.MedicationID,
                        IngredientID = ingredientId
                    };
                    _context.MedicationActiveIngredients.Add(medicationIngredient);
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            

            ViewData["DosageFormID"] = new SelectList(_context.DosageForm, "DosageFormID", "Form", medicationRecords.DosageFormID);
            ViewBag.ActiveIngredients = _context.ActiveIngredientRecords.ToList();
            return View(medicationRecords);
        }



        // GET: MedicationRecords/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medicationRecords = await _context.MedicationRecords.FindAsync(id);
            if (medicationRecords == null)
            {
                return NotFound();
            }
            ViewData["DosageFormID"] = new SelectList(_context.DosageForm, "DosageFormID", "Form", medicationRecords.DosageFormID);
            return View(medicationRecords);
        }

        // POST: MedicationRecords/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MedicationID,MedicationName,DosageFormID,Schedule,QuantityOnHand,ReOrderLevel")] MedicationRecords medicationRecords)
        {
            if (id != medicationRecords.MedicationID)
            {
                return NotFound();
            }

            
                try
                {
                    _context.Update(medicationRecords);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MedicationRecordsExists(medicationRecords.MedicationID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            
            ViewData["DosageFormID"] = new SelectList(_context.DosageForm, "DosageFormID", "Form", medicationRecords.DosageFormID);
            return View(medicationRecords);
        }

        // GET: MedicationRecords/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medicationRecords = await _context.MedicationRecords
                .Include(m => m.DosageForm)
                .FirstOrDefaultAsync(m => m.MedicationID == id);
            if (medicationRecords == null)
            {
                return NotFound();
            }

            return View(medicationRecords);
        }

        // POST: MedicationRecords/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var medicationRecords = await _context.MedicationRecords.FindAsync(id);
            if (medicationRecords != null)
            {
                _context.MedicationRecords.Remove(medicationRecords);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MedicationRecordsExists(int id)
        {
            return _context.MedicationRecords.Any(e => e.MedicationID == id);
        }
    }
}
