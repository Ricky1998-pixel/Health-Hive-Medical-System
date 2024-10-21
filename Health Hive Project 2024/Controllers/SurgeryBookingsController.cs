using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Health_Hive_Project_2024.Data;
using Health_Hive_Project_2024.Models;
//using Health_Hive_Project_2024.Migrations;
using Microsoft.AspNetCore.Identity;
using System.Net.Mail;
using System.Net;
using System.Net.Mime;

namespace Health_Hive_Project_2024.Controllers
{
    public class SurgeryBookingsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor http;

        //code for email
        private readonly IEmailService _emailService;

        private readonly ILogger<SurgeryBookingsController> _logger;


        public SurgeryBookingsController(ApplicationDbContext context, IHttpContextAccessor http, IEmailService emailService, ILogger<SurgeryBookingsController> logger)
        {
            _context = context;
            this.http = http;
            _emailService = emailService; // Injected Email Service
            _logger = logger; // Initialize the logger

        }

        [HttpGet("SurgeryBookings/SurgeonReports")]
        // Action for generating the surgery report
        public IActionResult SurgeonReports(DateTime? startDate, DateTime? endDate)
        {
            // If no dates are provided, default to the current month
            if (startDate == null)
                startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1); // Start of the current month

            if (endDate == null)
                endDate = DateTime.Now; // Today's date

            // Adjust the endDate to include the entire day (until 23:59:59)
            endDate = endDate.Value.Date.AddDays(1).AddTicks(-1);

            // Fetch the logged-in surgeon's identity (using username)
            var userName = User.Identity.Name;

            // Find the surgeon's record in the database
            var surgeon = _context.MedicalProfessionalRecords
                                  .FirstOrDefault(m => m.UserName == userName); // Changed to UserName instead of EmailAddress

            if (surgeon == null)
            {
                return NotFound("Surgeon not found.");
            }

            // Fetch surgeries for the logged-in surgeon within the specified date range
            var surgeries = _context.SurgeryBooking
                                    .Where(s => s.SurgeonID == surgeon.Id // Use the primary key field (Id from IdentityUser)
                                                && s.SurgeryDate >= startDate
                                                && s.SurgeryDate <= endDate)
                                    .Select(s => new
                                    {
                                        s.SurgeryDate,
                                        PatientName = $"{s.Patient.Name} {s.Patient.Surname}",
                                        TreatmentCodes = s.SurgeryBookingTreatments
                                                        .Select(t => t.TreatmentRecords.TreatmentCode).ToList()
                                    })
                                    .ToList();

            // Calculate treatment summary
            var treatmentSummary = _context.SurgeryBookingTreatments
                .Where(sbt => sbt.SurgeryBooking.SurgeonID == surgeon.Id
                              && sbt.SurgeryBooking.SurgeryDate >= startDate
                              && sbt.SurgeryBooking.SurgeryDate <= endDate)
                .GroupBy(t => t.TreatmentRecords.TreatmentCode)
                .Select(g => new
                {
                    TreatmentCode = g.Key,
                    TotalSurgeries = g.Count()
                })
                .ToList();

            // Pass the required data to the view
            ViewBag.SurgeonName = $"{surgeon.Name} {surgeon.Surname}";
            ViewBag.Surgeries = surgeries;
            ViewBag.TreatmentSummaries = treatmentSummary;
            ViewBag.StartDate = startDate;
            ViewBag.EndDate = endDate;

            return View();
        }

        //public async Task<IActionResult> Index()
        //{
        //    // Fetch SurgeryBookings with related entities
        //    var surgeryBookings = await _context.SurgeryBooking
        //        .Include(sb => sb.Surgeon)
        //        .Include(sb => sb.Patient)
        //        .Include(sb => sb.Anaesthesiologist)
        //        .Include(sb => sb.Theatre)
        //        .Include(sb => sb.SurgeryBookingTreatments)
        //            .ThenInclude(sbt => sbt.TreatmentRecords)
        //        .ToListAsync();



        //    return View(surgeryBookings);
        //}

        //public IActionResult Index(string searchTerm)
        //{
        //    // Fetch the logged-in surgeon's identity (using username)
        //    var userName = User.Identity.Name;

        //    // Find the surgeon's record in the database
        //    var surgeon = _context.MedicalProfessionalRecords
        //                          .FirstOrDefault(m => m.UserName == userName); // Changed to UserName instead of EmailAddress



        //    var surgeryBookings = _context.SurgeryBooking
        //        .Include(sb => sb.Patient)
        //        .Include(sb => sb.Surgeon)
        //        .Include(sb => sb.Anaesthesiologist)
        //        .Include(sb => sb.Theatre)
        //        .Include(sb => sb.SurgeryBookingTreatments)
        //        .ThenInclude(t => t.TreatmentRecords)
        //        .AsQueryable();

        //    // If searchTerm is provided, filter based on multiple fields
        //    if (!string.IsNullOrEmpty(searchTerm))
        //    {
        //        // Define the allowed date formats
        //        string[] dateFormats = { "yyyy-MM-dd", "MM/dd/yyyy", "dd/MM/yyyy" };

        //        // Try to parse searchTerm as a date in the provided formats
        //        DateTime surgeryDate;
        //        bool isDate = DateTime.TryParseExact(searchTerm, dateFormats,
        //                                             System.Globalization.CultureInfo.InvariantCulture,
        //                                             System.Globalization.DateTimeStyles.None,
        //                                             out surgeryDate);

        //        surgeryBookings = surgeryBookings.Where(sb =>
        //            sb.Patient.PatientIDNumber.Contains(searchTerm) ||
        //            sb.Patient.Name.Contains(searchTerm) ||
        //            sb.Patient.Surname.Contains(searchTerm) ||
        //            (isDate && sb.SurgeryDate.Date == surgeryDate) || // Compare date if parsed
        //            sb.Session.Contains(searchTerm) ||
        //            sb.Theatre.TheatreName.Contains(searchTerm));
        //    }

        //    return View(surgeryBookings.ToList());
        //}


        public IActionResult Index(string searchTerm)
        {
            // Fetch the logged-in surgeon's identity (using username)
            var userName = User.Identity.Name;

            // Find the surgeon's record in the database
            var surgeon = _context.MedicalProfessionalRecords
                                  .FirstOrDefault(m => m.UserName == userName);

            if (surgeon == null)
            {
                // Handle the case where the surgeon is not found
                return RedirectToAction("Error", "Home", new { message = "Surgeon not found" });
            }

            var surgeryBookings = _context.SurgeryBooking
                .Include(sb => sb.Patient)
                .Include(sb => sb.Surgeon)
                .Include(sb => sb.Anaesthesiologist)
                .Include(sb => sb.Theatre)
                .Include(sb => sb.SurgeryBookingTreatments)
                .ThenInclude(t => t.TreatmentRecords)
                .Where(sb => sb.Surgeon.Id == surgeon.Id) // Filter by the logged-in surgeon's ID
                .AsQueryable();

            // If searchTerm is provided, filter based on multiple fields
            if (!string.IsNullOrEmpty(searchTerm))
            {
                // Define the allowed date formats
                string[] dateFormats = { "yyyy-MM-dd", "MM/dd/yyyy", "dd/MM/yyyy" };
                // Try to parse searchTerm as a date in the provided formats
                DateTime surgeryDate;
                bool isDate = DateTime.TryParseExact(searchTerm, dateFormats,
                                                     System.Globalization.CultureInfo.InvariantCulture,
                                                     System.Globalization.DateTimeStyles.None,
                                                     out surgeryDate);
                surgeryBookings = surgeryBookings.Where(sb =>
                    sb.Patient.PatientIDNumber.Contains(searchTerm) ||
                    sb.Patient.Name.Contains(searchTerm) ||
                    sb.Patient.Surname.Contains(searchTerm) ||
                    (isDate && sb.SurgeryDate.Date == surgeryDate) || // Compare date if parsed
                    sb.Session.Contains(searchTerm) ||
                    sb.Theatre.TheatreName.Contains(searchTerm));
            }

            return View(surgeryBookings.ToList());
        }






        // GET: SurgeryBookings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var surgeryBooking = await _context.SurgeryBooking
                .Include(s => s.Anaesthesiologist)
                .Include(s => s.Patient)
                .Include(s => s.Surgeon)
                .Include(s => s.Theatre)
                .FirstOrDefaultAsync(m => m.SurgeryID == id);
            if (surgeryBooking == null)
            {
                return NotFound();
            }

            return View(surgeryBooking);
        }

        ////// GET: SurgeryBookings/Create
        public IActionResult Create(string patientID = null)
        {
            // Get the current user's username
            var userName = User.Identity.Name;

            // Find the corresponding surgeon based on username (changed to use Id instead of MedicalProfessionalID)
            var surgeon = _context.MedicalProfessionalRecords
                .Where(m => m.Specialization == SpecializationType.Surgeon && m.UserName == userName) // Using UserName, assuming it's more accurate here
                .Select(m => new { m.Id, FullName = m.Name + " " + m.Surname }) 
                .FirstOrDefault();

            // Populate Anaesthesiologist dropdown
            ViewBag.AnaesthesiologistID = new SelectList(_context.MedicalProfessionalRecords
                .Where(m => m.Specialization == SpecializationType.Anesthesiologist)
                .Select(m => new { m.Id, FullName = m.Name + " " + m.Surname }), "Id", "FullName"); 

            // Populate Patient dropdown and pre-select if patientID is provided
            var patients = _context.Patients
                .Select(p => new { p.PatientID, FullName = p.Name + " " + p.Surname })
                .ToList();

            ViewBag.PatientID = new SelectList(patients, "PatientID", "FullName", patientID);

            // Populate Theatre dropdown
            ViewBag.TheatreID = new SelectList(_context.OperatingTheatreRecords, "TheatreID", "TheatreName");

            // Populate Treatment Codes (adjusted formatting if using dropdown or checkbox list)
            var treatmentCodes = _context.TreatmentRecords.ToList();
            ViewBag.TreatmentCodes = treatmentCodes;

            // Set the surgeon dropdown, pre-selecting the logged-in Surgeon if found
            ViewData["SurgeonID"] = surgeon != null
                ? new SelectList(new List<dynamic> { surgeon }, "Id", "FullName", surgeon.Id) // Changed MedicalProfessionalID to Id
                : new SelectList(Enumerable.Empty<dynamic>());

            // Check TempData and set ViewBag flag if needed (for modal display or notifications)
            ViewBag.ShowModal = TempData["ShowModal"] != null;

            return View();
        }








        // GET: SurgeryBookings/GetTreatmentCodes
        [HttpGet]
        public IActionResult GetTreatmentCodes()
        {
            var treatmentCodes = _context.TreatmentRecords
                .Select(t => new
                {
                    TreatmentID = t.TreatmentID,
                    TreatmentCode = t.TreatmentCode,
                    Description = t.Description
                })
                .ToList();

            return Json(treatmentCodes);
        }






        // POST: SurgeryBookings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("SurgeryID,SurgeonID,PatientID,SurgeryDate,Session,AnaesthesiologistID,TheatreID")] SurgeryBooking surgeryBooking, int[] treatmentIDs)
        //{

        //        // Save the SurgeryBooking entity
        //        _context.Add(surgeryBooking);
        //        await _context.SaveChangesAsync();

        //        // Save associated treatments
        //        if (treatmentIDs != null && treatmentIDs.Length > 0)
        //        {
        //            foreach (var treatmentID in treatmentIDs)
        //            {
        //                var surgeryBookingTreatment = new SurgeryBookingTreatment
        //                {
        //                    SurgeryID = surgeryBooking.SurgeryID,
        //                    TreatmentID = treatmentID
        //                };
        //                _context.SurgeryBookingTreatments.Add(surgeryBookingTreatment);
        //            }
        //            await _context.SaveChangesAsync();
        //        }

        //    // Fetch patient, surgeon, anaesthesiologist, and theatre details for email
        //    var patient = await _context.Patients.FindAsync(surgeryBooking.PatientID);
        //    //var surgeon = await _context.MedicalProfessionalRecords.FindAsync(surgeryBooking.SurgeonID);
        //    //var anaesthesiologist = await _context.MedicalProfessionalRecords.FindAsync(surgeryBooking.AnaesthesiologistID);
        //    //var theatre = await _context.OperatingTheatreRecords.FindAsync(surgeryBooking.TheatreID);

        //    if (patient != null )
        //    {
        //        string patientEmail = patient.EmailAddress;
        //        string patientName = patient.Name;
        //        string patientSurname = patient.Surname;

        //        // Send email with TheatreID
        //        await SendEmailAsync(
        //            patientEmail,
        //            patientName,
        //            patientSurname,
        //            surgeryBooking.SurgeryDate
        //            /*theatre.TheatreName*/ // Pass Theatre name or ID
        //        );
        //    }
        //    else
        //        {
        //            _logger.LogError("Failed to send email. Missing patient, surgeon, or anaesthesiologist details.");
        //        }

        //        // Set TempData to show modal for success
        //        TempData["ShowModal"] = true;

        //        // Redirect to the Create view to indicate success
        //        return RedirectToAction("Create");


        //    // Repopulate dropdowns in case of validation error
        //    ViewBag.SurgeonID = new SelectList(_context.MedicalProfessionalRecords
        //        .Where(u => u.Specialization == SpecializationType.Surgeon)  // Filter surgeons based on SpecializationType
        //        .Select(u => new { u.Id, FullName = u.Name + " " + u.Surname }), "Id", "FullName");

        //    ViewBag.AnaesthesiologistID = new SelectList(_context.MedicalProfessionalRecords
        //        .Where(u => u.Specialization == SpecializationType.Anesthesiologist)  // Filter anaesthesiologists based on SpecializationType
        //        .Select(u => new { u.Id, FullName = u.Name + " " + u.Surname }), "Id", "FullName");

        //    ViewBag.PatientID = new SelectList(_context.Patients
        //        .Select(p => new { p.PatientID, FullName = p.Name + " " + p.Surname }), "PatientID", "FullName");

        //    ViewBag.TheatreID = new SelectList(_context.OperatingTheatreRecords, "TheatreID", "TheatreName");

        //    // Return the view again if validation fails
        //    return View(surgeryBooking);
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SurgeryID,SurgeonID,PatientID,SurgeryDate,Session,AnaesthesiologistID,TheatreID")] SurgeryBooking surgeryBooking, string SelectedTreatmentCodes)
        {
            
            
                // Save the SurgeryBooking entity
                _context.Add(surgeryBooking);
                await _context.SaveChangesAsync();

                // Save associated treatments
                if (!string.IsNullOrEmpty(SelectedTreatmentCodes))
                {
                    try
                    {
                        var treatmentIDs = System.Text.Json.JsonSerializer.Deserialize<List<int>>(SelectedTreatmentCodes);
                        foreach (var treatmentID in treatmentIDs)
                        {
                            var surgeryBookingTreatment = new SurgeryBookingTreatment
                            {
                                SurgeryID = surgeryBooking.SurgeryID,
                                TreatmentID = treatmentID
                            };
                            _context.SurgeryBookingTreatments.Add(surgeryBookingTreatment);
                        }
                        await _context.SaveChangesAsync();
                    }
                    catch (System.Text.Json.JsonException ex)
                    {
                        _logger.LogError($"Error parsing treatment codes: {ex.Message}");
                        ModelState.AddModelError("", "Error processing selected treatment codes.");
                        return View(surgeryBooking);
                    }
                }

                // Fetch patient details for email
                var patient = await _context.Patients.FindAsync(surgeryBooking.PatientID);
                if (patient != null)
                {
                    string patientEmail = patient.EmailAddress;
                    string patientName = patient.Name;
                    string patientSurname = patient.Surname;

                    // Send email
                    await SendEmailAsync(
                        patientEmail,
                        patientName,
                        patientSurname,
                        surgeryBooking.SurgeryDate
                    );
                }
                else
                {
                    _logger.LogError("Failed to send email. Missing patient details.");
                }

                // Set TempData to show modal for success
                TempData["ShowModal"] = true;

                // Redirect to the Create view to indicate success
                return RedirectToAction("Create");
            

            // Repopulate dropdowns in case of validation error
            ViewBag.SurgeonID = new SelectList(_context.MedicalProfessionalRecords
                .Where(u => u.Specialization == SpecializationType.Surgeon)
                .Select(u => new { u.Id, FullName = u.Name + " " + u.Surname }), "Id", "FullName");
            ViewBag.AnaesthesiologistID = new SelectList(_context.MedicalProfessionalRecords
                .Where(u => u.Specialization == SpecializationType.Anesthesiologist)
                .Select(u => new { u.Id, FullName = u.Name + " " + u.Surname }), "Id", "FullName");
            ViewBag.PatientID = new SelectList(_context.Patients
                .Select(p => new { p.PatientID, FullName = p.Name + " " + p.Surname }), "PatientID", "FullName");
            ViewBag.TheatreID = new SelectList(_context.OperatingTheatreRecords, "TheatreID", "TheatreName");

            // Return the view again if validation fails
            return View(surgeryBooking);
        }





        //Action for sending email
        private async Task SendEmailAsync(string toEmail, string name, string surname, DateTime surgeryDate)
        {
            

            string subject = "Surgery Booking Confirmation - GROUP 16";

            // Format the email body with HTML
            string body = $@"
        <p>Dear {name} {surname},</p>
        <p>We hope this email finds you well. We are writing to confirm your upcoming surgery at Bay Breeze Hospital with the following details:</p>
        
        <p>
            <strong><img src='cid:calendarIcon' style='width:20px; height:20px; margin-right:5px;'> Surgery Date and Time:</strong> {surgeryDate.ToString("yyyy-MM-dd HH:mm")}
        </p>

        <p> Additional Information:</p>

<p>Pre-Surgery Instructions:</p>
<p>Please follow any pre-surgery instructions provided by your doctor, such as fasting requirements or medication guidelines.</p>

<p>Check-In Time:</p>
<p>You are requested to arrive at the hospital no later than 3 hours prior to the surgery for pre-operative assessments.</p>

<p>Contact Information:</p>
<p>Should you have any questions or require assistance, feel free to contact us at 041 583 2121 or email baybreeze@gmail.com.</p>

         <p> We will keep you informed of any updates, and we encourage you to reach out if you have concerns before the surgery.</p>

         <p> Thank you for choosing Bay Breeze Hospital for your medical care. We are committed to ensuring your experience is as smooth and comfortable as possible.</p>
        
        <p>Best regards,<br>The Bay Breeze Team. Powered by Health Hive Systems</p>";

            // Create the email service instance to use configuration settings
            var smtpHost = "smtp.gmail.com";
            int smtpPort = 587;
            string fromAddress = "hivehealth628@gmail.com";
            string smtpUsername = "hivehealth628@gmail.com";
            string smtpPassword = "pcbbjzzjndrmarqi";

            var mailMessage = new MailMessage(fromAddress, toEmail)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true // Important: Set to true to render HTML
            };

            // Add the icons as inline attachments
            var calendarIcon = new LinkedResource(@"wwwroot/images/Calendar.png", "image/png")
            {
                ContentId = "calendarIcon"
            };
            var theatreIcon = new LinkedResource(@"wwwroot/images/theatre.png", "image/png")
            {
                ContentId = "theatreIcon"
            };
            //var anaesthesiologistIcon = new LinkedResource(@"wwwroot/images/anaes.png", "image/png")
            //{
            //    ContentId = "anaesthesiologistIcon"
            //};
            //var surgeontIcon = new LinkedResource(@"wwwroot/images/surgeon.png", "image/png")
            //{
            //    ContentId = "surgeontIcon"
            //};

            var alternateView = AlternateView.CreateAlternateViewFromString(body, null, MediaTypeNames.Text.Html);
            alternateView.LinkedResources.Add(calendarIcon);
            alternateView.LinkedResources.Add(theatreIcon);
            mailMessage.AlternateViews.Add(alternateView);

            using (var smtpClient = new SmtpClient(smtpHost, smtpPort))
            {
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential(smtpUsername, smtpPassword);
                smtpClient.EnableSsl = true;

                await smtpClient.SendMailAsync(mailMessage);
            }
        }













        // GET: SurgeryBookings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var surgeryBooking = await _context.SurgeryBooking.FindAsync(id);
            if (surgeryBooking == null)
            {
                return NotFound();
            }
            ViewData["AnaesthesiologistID"] = new SelectList(_context.MedicalProfessionalRecords, "Id", "FullName", surgeryBooking.AnaesthesiologistID);
            ViewData["PatientID"] = new SelectList(_context.Patients, "PatientID", "Address", surgeryBooking.PatientID);
            ViewData["SurgeonID"] = new SelectList(_context.MedicalProfessionalRecords, "Id", "FullName", surgeryBooking.SurgeonID);
            ViewData["TheatreID"] = new SelectList(_context.OperatingTheatreRecords, "TheatreID", "TheatreName", surgeryBooking.TheatreID);
            return View(surgeryBooking);
        }






        // POST: SurgeryBookings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SurgeryID,SurgeonID,PatientID,SurgeryDate,Session,AnaesthesiologistID,TheatreID")] SurgeryBooking surgeryBooking)
        {
            if (id != surgeryBooking.SurgeryID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(surgeryBooking);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SurgeryBookingExists(surgeryBooking.SurgeryID))
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
            ViewData["AnaesthesiologistID"] = new SelectList(_context.MedicalProfessionalRecords, "MedicalProfessionalID", "FullName", surgeryBooking.AnaesthesiologistID);
            ViewData["PatientID"] = new SelectList(_context.Patients, "PatientID", "Address", surgeryBooking.PatientID);
            ViewData["SurgeonID"] = new SelectList(_context.MedicalProfessionalRecords, "MedicalProfessionalID", "FullName", surgeryBooking.SurgeonID);
            ViewData["TheatreID"] = new SelectList(_context.OperatingTheatreRecords, "TheatreID", "TheatreName", surgeryBooking.TheatreID);
            return View(surgeryBooking);
        }





        // GET: SurgeryBookings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var surgeryBooking = await _context.SurgeryBooking
                .Include(s => s.Anaesthesiologist)
                .Include(s => s.Patient)
                .Include(s => s.Surgeon)
                .Include(s => s.Theatre)
                .FirstOrDefaultAsync(m => m.SurgeryID == id);
            if (surgeryBooking == null)
            {
                return NotFound();
            }

            return View(surgeryBooking);
        }




        // POST: SurgeryBookings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var surgeryBooking = await _context.SurgeryBooking.FindAsync(id);
            if (surgeryBooking != null)
            {
                _context.SurgeryBooking.Remove(surgeryBooking);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        // POST: SurgeryBookings/Delete/5

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> MoveToCompleteSurgeries(int id)
        //{
        //    try
        //    {
        //        // Retrieve the surgery booking
        //        var surgeryBooking = await _context.SurgeryBooking
        //            .Include(s => s.SurgeryBookingTreatments)
        //            .FirstOrDefaultAsync(m => m.SurgeryID == id);

        //        if (surgeryBooking == null)
        //        {
        //            return Json(new { success = false, message = "Surgery booking not found." });
        //        }

        //        // Create a new CompleteSurgery record
        //        // Create a new CompleteSurgery record
        //        var completeSurgery = new Health_Hive_Project_2024.Models.CompleteSurgery
        //        {
        //            SurgeryID = surgeryBooking.SurgeryID,
        //            SurgeonID = int.TryParse(surgeryBooking.SurgeonID, out int surgeonId) ? (int?)surgeonId : null,
        //            PatientID = surgeryBooking.PatientID,
        //            AnaesthesiologistID = int.TryParse(surgeryBooking.AnaesthesiologistID, out int anaesthesiologistId) ? (int?)anaesthesiologistId : null,
        //            TheatreID = surgeryBooking.TheatreID,
        //            SurgeryDate = surgeryBooking.SurgeryDate,
        //            Session = surgeryBooking.Session,
        //            SurgeryBookingTreatments = surgeryBooking.SurgeryBookingTreatments.ToList() // Copy treatments
        //        };


        //        // Use a transaction for atomicity
        //        using (var transaction = await _context.Database.BeginTransactionAsync())
        //        {
        //            try
        //            {
        //                // Add the record to the CompleteSurgeries table
        //                await _context.CompleteSurgeries.AddAsync(completeSurgery);

        //                // Remove the record from the SurgeryBooking table
        //                _context.SurgeryBooking.Remove(surgeryBooking);

        //                // Save changes to the database
        //                await _context.SaveChangesAsync();

        //                // Commit the transaction
        //                await transaction.CommitAsync();
        //            }
        //            catch (Exception ex)
        //            {
        //                // Rollback the transaction in case of error
        //                await transaction.RollbackAsync();

        //                // Log the exception for debugging
        //                _logger.LogError(ex, "Error moving surgery booking to completed. SurgeryID: {SurgeryID}", id);

        //                return Json(new { success = false, message = "An error occurred while completing the surgery: " + ex.Message });
        //            }
        //        }

        //        return Json(new { success = true });
        //    }
        //    catch (Exception ex)
        //    {
        //        // Log the exception if necessary
        //        _logger.LogError(ex, "Unexpected error while completing the surgery. SurgeryID: {SurgeryID}", id);
        //        return Json(new { success = false, message = "An unexpected error occurred." });
        //    }
        //}


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MoveToCompleteSurgeries(int id)
        {
            try
            {
                // Retrieve the surgery booking including necessary related data
                var surgeryBooking = await _context.SurgeryBooking
                    .Include(s => s.SurgeryBookingTreatments)
                    .Include(s => s.Surgeon) // Including related entities
                    .Include(s => s.Patient)
                    .Include(s => s.Anaesthesiologist)
                    .Include(s => s.Theatre)
                    .Include(sb => sb.SurgeryBookingTreatments)
                .ThenInclude(t => t.TreatmentRecords)
                    .FirstOrDefaultAsync(m => m.SurgeryID == id);

                if (surgeryBooking == null)
                {
                    return Json(new { success = false, message = "Surgery booking not found." });
                }

                // Create a new CompleteSurgery record
                var completeSurgery = new Health_Hive_Project_2024.Models.CompleteSurgery
                {
                    SurgeryID = surgeryBooking.SurgeryID,
                    SurgeonID = surgeryBooking.SurgeonID, // Nullable, so no issue assigning
                    PatientID = surgeryBooking.PatientID, // Nullable, so no issue assigning
                    AnaesthesiologistID = surgeryBooking.AnaesthesiologistID, // Nullable, so no issue assigning
                    TheatreID = surgeryBooking.TheatreID, // Nullable, so no issue assigning
                    SurgeryDate = surgeryBooking.SurgeryDate,
                    Session = surgeryBooking.Session,
                    SurgeryBookingTreatments = surgeryBooking.SurgeryBookingTreatments.ToList(), // Copy treatments

                    // Setting the navigation properties explicitly (if needed)
                    Surgeon = surgeryBooking.Surgeon,
                    Patient = surgeryBooking.Patient,
                    Anaesthesiologist = surgeryBooking.Anaesthesiologist,
                    Theatre = surgeryBooking.Theatre
                };

                // Add the record to the CompleteSurgeries table
                _context.CompleteSurgeries.Add(completeSurgery);

                // Remove the record from the SurgeryBooking table
                _context.SurgeryBooking.Remove(surgeryBooking);

                // Save changes to the database
                await _context.SaveChangesAsync();

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                // Log the exception if necessary
                return Json(new { success = false, message = "An error occurred while completing the surgery." });
            }
        }









        private bool SurgeryBookingExists(int id)
        {
            return _context.SurgeryBooking.Any(e => e.SurgeryID == id);
        }
    }
}