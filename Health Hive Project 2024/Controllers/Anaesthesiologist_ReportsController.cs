using Health_Hive_Project_2024.Data;
using Health_Hive_Project_2024.Models;
using Health_Hive_Project_2024.Views.Anaesthesiologist_Reports;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;
using System.Security.Claims;
using Document = iTextSharp.text.Document;

namespace Health_Hive_Project_2024.Controllers
{
    public class Anaesthesiologist_ReportsController : Controller
    {

        private readonly ApplicationDbContext _context;
        private readonly UserManager<MedicalProfessionalRecords> _userManager;
        public Anaesthesiologist_ReportsController(ApplicationDbContext context, UserManager<MedicalProfessionalRecords> userManager)
        {
            _context = context;
            this._userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        //public IActionResult GetReport()
        //{
        //    return View();
        //}





        [HttpPost]
        public async Task<IActionResult> GenerateReport(DateTime startDate, DateTime endDate, OrderStatus? selectedStatus)
        {
            // Get the ID of the logged-in user
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userI = await this._userManager.GetUserAsync(User);
            if (userI == null)
            {
                return NotFound();
            }

            string lastName = userI.Name;
            string firstName = userI.Surname;

            var userFullName = firstName + "\t" + lastName;

            // Fetch orders based on the date range and logged-in user, ensuring there are medications
            var orders = await _context.GetMedicationOrder
                .Where(o => o.Date >= startDate && o.Date <= endDate && o.Id == userId && o.GetMedicationData.Any())
                .Include(o => o.Patient)
                .Include(o => o.GetMedicationData)
                .ThenInclude(m => m.Medication)
                .ToListAsync();

            // Apply the status filter if selected
            if (selectedStatus.HasValue)
            {
                orders = orders.Where(o => o.Status == selectedStatus.Value).ToList();
            }

            // Create the ViewModel with the orders and user's name
            var viewModel = new ReportViewModel
            {
                StartDate = startDate,
                EndDate = endDate,
                Orders = orders,
                LoggedInUserFullName = userFullName,
                SelectedStatus = selectedStatus,
                Statuses = Enum.GetValues(typeof(OrderStatus))
                    .Cast<OrderStatus>()
                    .Select(s => new SelectListItem
                    {
                        Value = s.ToString(),
                        Text = s.ToString()
                    }).ToList()
            };

            // Pass the data to the view
            return View("GetReport", viewModel);  // Reuse the same view for displaying the report
        }




        public IActionResult GetReport()
        {
            // Initialize the model with default dates (e.g., today)
            var viewModel = new ReportViewModel
            {
                StartDate = DateTime.Today,
                EndDate = DateTime.Today
            };

            // Return the view with the model
            return View(viewModel);
        }


        [HttpPost]
        public async Task<IActionResult> DownloadReportPdf(DateTime startDate, DateTime endDate)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userI = await this._userManager.GetUserAsync(User);
            if (userI == null)
            {
                return NotFound();
            }

            string lastName = userI.Name;
            string firstName = userI.Surname;

            var userFullName = firstName + "\t" + lastName;

            // Fetch orders based on the date range
            var orders = await _context.GetMedicationOrder
                .Where(o => o.Date >= startDate && o.Date <= endDate && o.Id == userId)
                .Include(o => o.Patient)
                .Include(o => o.GetMedicationData)
                .ThenInclude(m => m.Medication)
                .ToListAsync();

            // Generate the PDF report
            byte[] pdfBytes = GeneratePDFReport(orders, startDate, endDate, userFullName);

            // Return the PDF as a file download
            return File(pdfBytes, "application/pdf", "AnestheticReport.pdf");
        }

        private byte[] GeneratePDFReport(List<OrderMedications> orders, DateTime startDate, DateTime endDate, string userFullName)
        {
            using (var memoryStream = new MemoryStream())
            {
                Document doc = new Document(PageSize.A4);
                PdfWriter writer = PdfWriter.GetInstance(doc, memoryStream);
                doc.Open();

                writer.PageEvent = new PdfPageNumberHelper();
                // Define fonts and colors
                var titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 20, BaseColor.BLUE);
                var headerFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12, BaseColor.WHITE);
                var cellFont = FontFactory.GetFont(FontFactory.HELVETICA, 11, BaseColor.BLACK);
                var summaryFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12, BaseColor.DARK_GRAY);
                var dateFont = FontFactory.GetFont(FontFactory.HELVETICA, 10, BaseColor.GRAY);

                // Add title with centered alignment
                var title = new Paragraph("Anesthetic Report", titleFont)
                {
                    Alignment = Element.ALIGN_CENTER
                };
                doc.Add(title);

                // Add a line break
                doc.Add(new Paragraph("\n"));

                // Add logged-in user's full name, centered
                Paragraph userParagraph = new Paragraph($"\nDr {userFullName}\n", FontFactory.GetFont(FontFactory.HELVETICA, 12));
                userParagraph.Alignment = Element.ALIGN_CENTER;
                doc.Add(userParagraph);

                // Add date range and report generation date
                Paragraph dateParagraph = new Paragraph($"Date Range: {startDate:dd MMM yyyy} - {endDate:dd MMM yyyy}", dateFont);
                dateParagraph.Alignment = Element.ALIGN_RIGHT;
                doc.Add(dateParagraph);

                Paragraph generatedDateParagraph = new Paragraph($"Report Generated: {DateTime.Now:dd MMM yyyy}", dateFont);
                generatedDateParagraph.Alignment = Element.ALIGN_RIGHT;
                doc.Add(generatedDateParagraph);

                // Add a line break
                doc.Add(new Paragraph("\n"));

                // Define method to add status subsection
                void AddStatusSubsection(string statusTitle, OrderStatus status)
                {
                    // Add subsection title
                    doc.Add(new Paragraph($"\n{statusTitle} Orders:", summaryFont));
                    doc.Add(new Paragraph("\n"));

                    // Create table for the status
                    PdfPTable table = new PdfPTable(3) { WidthPercentage = 100 };
                    table.SetWidths(new float[] { 1f, 2f, 1f });

                    // Add table headers with background color
                    PdfPCell headerCell1 = new PdfPCell(new Phrase("Patient Name", headerFont)) { BackgroundColor = BaseColor.DARK_GRAY, Padding = 5 };
                    PdfPCell headerCell2 = new PdfPCell(new Phrase("Medications", headerFont)) { BackgroundColor = BaseColor.DARK_GRAY, Padding = 5 };
                    PdfPCell headerCell3 = new PdfPCell(new Phrase("Date", headerFont)) { BackgroundColor = BaseColor.DARK_GRAY, Padding = 5 };

                    table.AddCell(headerCell1);
                    table.AddCell(headerCell2);
                    table.AddCell(headerCell3);

                    // Filter orders by the current status
                    var filteredOrders = orders.Where(o => o.Status == status).ToList();

                    // Add rows for patient medications
                    foreach (var order in filteredOrders)
                    {
                        bool isFirstMedication = true;

                        foreach (var medData in order.GetMedicationData)
                        {
                            if (isFirstMedication)
                            {
                                PdfPCell patientCell = new PdfPCell(new Phrase(order.Patient.Name, cellFont))
                                {
                                    Rowspan = order.GetMedicationData.Count,
                                    Padding = 5
                                };
                                table.AddCell(patientCell);
                                isFirstMedication = false;
                            }

                            PdfPCell medicationCell = new PdfPCell(new Phrase($"{medData.Medication.MedicationName} (Qty: {medData.Quantity})", cellFont))
                            {
                                Padding = 5
                            };
                            table.AddCell(medicationCell);

                            PdfPCell dateCell = new PdfPCell(new Phrase(order.Date.ToString("dd MMM yyyy"), cellFont))
                            {
                                Padding = 5
                            };
                            table.AddCell(dateCell);
                        }
                    }

                    doc.Add(table);
                }

                // Add subsections for each status
                AddStatusSubsection("Ordered", OrderStatus.Ordered);
                AddStatusSubsection("Dispensed", OrderStatus.Dispensed);
                AddStatusSubsection("Rejected", OrderStatus.Rejected);
                AddStatusSubsection("Received", OrderStatus.Received);

                // Summary section
                doc.Add(new Paragraph("\nTotal Patients: " + orders.Select(o => o.PatientID).Distinct().Count(), summaryFont));
                doc.Add(new Paragraph("Summary per Medication:", summaryFont));
                doc.Add(new Paragraph("\n"));

                // Create the summary table
                PdfPTable summaryTable = new PdfPTable(2) { WidthPercentage = 100 };
                summaryTable.SetWidths(new float[] { 2f, 1f });

                PdfPCell summaryHeader1 = new PdfPCell(new Phrase("Medicine", headerFont)) { BackgroundColor = BaseColor.DARK_GRAY, Padding = 5 };
                PdfPCell summaryHeader2 = new PdfPCell(new Phrase("Qty Ordered", headerFont)) { BackgroundColor = BaseColor.DARK_GRAY, Padding = 5 };

                summaryTable.AddCell(summaryHeader1);
                summaryTable.AddCell(summaryHeader2);

                // Group and sum medication quantities
                var medicineSummary = orders
                    .SelectMany(o => o.GetMedicationData)
                    .GroupBy(m => m.Medication.MedicationName)
                    .Select(g => new { Medicine = g.Key, TotalQuantity = g.Sum(m => m.Quantity) });

                foreach (var medSummary in medicineSummary)
                {
                    PdfPCell medNameCell = new PdfPCell(new Phrase(medSummary.Medicine, cellFont)) { Padding = 5 };
                    PdfPCell medQtyCell = new PdfPCell(new Phrase(medSummary.TotalQuantity.ToString(), cellFont)) { Padding = 5 };

                    summaryTable.AddCell(medNameCell);
                    summaryTable.AddCell(medQtyCell);
                }

                doc.Add(summaryTable);

                doc.Close();
                return memoryStream.ToArray();
            }
        }


    }
















    //[HttpPost]
    //public async Task<IActionResult> GenerateReport(DateTime startDate, DateTime endDate)
    //{
    //    // Fetch the logged-in user (Anaesthesiologist)
    //    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

    //    // Fetch all OrderMedications based on date range and user
    //    var orders = await _context.GetMedicationOrder
    //        .Where(o => o.Date >= startDate && o.Date <= endDate && o.Id == userId)
    //        .Include(o => o.Patient)
    //        .Include(o => o.GetMedicationData)
    //        .ThenInclude(m => m.Medication)
    //        .ToListAsync();

    //    // Call method to generate PDF
    //    byte[] pdfBytes = GeneratePDFReport(orders, startDate, endDate);

    //    // Return the PDF as a file to download
    //    return File(pdfBytes, "application/pdf", "AnestheticReport.pdf");
    //}

    //private byte[] GeneratePDFReport(List<OrderMedications> orders, DateTime startDate, DateTime endDate)
    //{
    //    using (var memoryStream = new MemoryStream())
    //    {
    //        Document doc = new Document(PageSize.A4);
    //        PdfWriter writer = PdfWriter.GetInstance(doc, memoryStream);

    //        doc.Open();
    //        // Add title and headers
    //        doc.Add(new Paragraph("Anesthetic Report"));
    //        doc.Add(new Paragraph("\n\n"));
    //        doc.Add(new Paragraph($"Date Range: {startDate.ToString("dd MMM yyyy")} - {endDate.ToString("dd MMM yyyy")} \t \t \t Report Generated: {DateTime.Now.ToString("dd MMM yyyy")}"));
    //        //doc.Add(new Paragraph($"Report Generated: {DateTime.Now.ToString("dd MMM yyyy")}"));
    //        doc.Add(new Paragraph("\n"));

    //        PdfPTable table = new PdfPTable(4);
    //        table.AddCell("Date");
    //        table.AddCell("Patient");
    //        table.AddCell("Medication");
    //        table.AddCell("Quantity");

    //        foreach (var order in orders)
    //        {
    //            foreach (var medData in order.GetMedicationData)
    //            {
    //                table.AddCell(order.Date.ToString("dd MMM yyyy"));
    //                table.AddCell(order.Patient.Name); // Assuming PatientName exists
    //                table.AddCell(medData.Medication.MedicationName); // Assuming MedicationName exists
    //                table.AddCell(medData.Quantity.ToString());
    //            }
    //        }

    //        doc.Add(table);

    //        // Summary section
    //        doc.Add(new Paragraph("\nTotal Patients: " + orders.Select(o => o.PatientID).Distinct().Count()));
    //        doc.Add(new Paragraph("Summary per Medicine:"));
    //        PdfPTable summaryTable = new PdfPTable(2);
    //        summaryTable.AddCell("Medicine");
    //        summaryTable.AddCell("Qty Ordered");

    //        // Group and sum medication quantities
    //        var medicineSummary = orders
    //            .SelectMany(o => o.GetMedicationData)
    //            .GroupBy(m => m.Medication.MedicationName)
    //            .Select(g => new { Medicine = g.Key, TotalQuantity = g.Sum(m => m.Quantity) });

    //        foreach (var medSummary in medicineSummary)
    //        {
    //            summaryTable.AddCell(medSummary.Medicine);
    //            summaryTable.AddCell(medSummary.TotalQuantity.ToString());
    //        }

    //        doc.Add(summaryTable);
    //        doc.Close();

    //        return memoryStream.ToArray();
    //    }
    //}
}

