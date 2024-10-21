using Health_Hive_Project_2024.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Health_Hive_Project_2024.Data.ViewModels;
using Health_Hive_Project_2024.Models;
using SQLitePCL;
using MimeKit;
using MailKit.Net.Smtp;
namespace Health_Hive_Project_2024.Controllers
{
    //[Authorize(Roles = "Nurse")]
    public class NurseController : Controller
    {
        private readonly ApplicationDbContext _context;

        public NurseController(ApplicationDbContext context)
        {
            _context = context;
        }



        public async Task<IActionResult> Index() // need to pass notification numbers its not !DONE
        {
            var book = await _context.SurgeryBooking.Where(x => x.IsActive).CountAsync();
            var addmit = await _context.PatientAdmissions.Where(x=>x.DischargeDate == null).CountAsync();

            var po = _context.PrescriptionMedications.Include(c => c.PrescriptionRecords).Where(x => (x.PrescriptionRecords.PrescriptionStatus == "Received" || x.PrescriptionRecords.PrescriptionStatus == "Dispensed")).AsEnumerable().OrderByDescending(c=>c.AdministeredDate);

            var admit1 = _context.PatientAdmissions.Where(x => x.Id == HttpContext.Session.GetString("userid") && x.DischargeDate == null);

            var m = (from p in po
                    join a in admit1 on p.PrescriptionRecords.PatientID equals a.PatientID
                    select new
                    {
                        d = p.PrescriptionRecords.PatientID,
                        id = p.PrescriptionID
                    }).DistinctBy(c=>c.id).Count();
            DashboardCount d = new()
            {
                Administer = m,
                Booking = book,
                Admitted = addmit,
                Discharge = await admit1.Where(x=>x.DischargeDate.Value.Date == DateTime.Now.Date).CountAsync(),
            };
            // Retrieve nurse's full name from session and pass it to the view
            var nurseFullName = HttpContext.Session.GetString("user_full_name");
            ViewBag.NurseName = nurseFullName ?? "Nurse";  // Default to "Nurse" if name is not available

            return View(d);
        }

        public IActionResult ViewAdmitedPatients()
        {
            // Fetch the required data from your database
            var patients = _context.Patients.ToList();  // Fetch all patients
            var patientVitals = _context.PatientVitals.ToList();  // Fetch all patient vitals

            // Assume you need to convert the Patient model to PatientVMM manually
            var patientVMM = patients.Select(p => new PatientVMM
            {
                PatientID = p.PatientID,  // Assuming PatientVMM has a similar property
                Name = p.Name,            // Map properties accordingly
                Surname = p.Surname,
                Address = p.Address,
                ContactNumber = p.ContactNumber,
                EmailAddress = p.EmailAddress,
                DateOfBirth = p.DateOfBirth,
                Gender = p.Gender
                // Map any other necessary fields from Patient to PatientVMM
            }).FirstOrDefault();  // Adjust this logic based on how you're selecting the patient

            // Construct the AdmitPat model (you may adjust how the data is assigned if needed)
            var admitPatViewModel = new AdmitPat
            {
                //Patient = patients.FirstOrDefault(),  // Replace with logic to fetch the specific patient if needed
                PatientVitals = patientVitals,  // All patient vitals
                Ward = 0,  // Modify as per actual data retrieval logic
                Bed = 0,   // Modify as per actual data retrieval logic
                Height = 0,  // Modify as per actual data retrieval logic
                Weight = 0,  // Modify as per actual data retrieval logic
                BookingID = ""  // Modify as per actual data retrieval logic
            };

            // Pass the ViewModel to the View
            return View(admitPatViewModel);
        }




        public async Task<IActionResult> Bookings()
        {
            var applicationDbContext = _context.SurgeryBooking.Include(s => s.Anaesthesiologist).Include(s => s.Patient).Include(s => s.Surgeon).Include(s => s.Theatre).Where(x => x.IsActive);
            return View(await applicationDbContext.ToListAsync());
        } 
       
        public async Task<IActionResult> Discharged()
        {
            var applicationDbContext = _context.PatientAdmissions.Include(p => p.Patient).Include(p => p.Ward).Include(p => p.Bed).Where(x => x.Id == HttpContext.Session.GetString("userid") && x.AdmissionDate.Date == DateTime.Now.Date && x.DischargeDate != null).AsEnumerable();
            return View(applicationDbContext);
        }

        [HttpPost]
        public async Task<IActionResult> Report(DateTime f, DateTime t)
        {
            var m = _context.PrescriptionMedications.Include(c => c.PrescriptionRecords).Include(c => c.PrescriptionRecords.Patient).Include(c => c.MedicationRecords).Where(c => c.PrescriptionRecords.NurseID == HttpContext.Session.GetString("userid") && c.AdministeredDate.Value.Date >= f.Date && c.AdministeredDate.Value.Date <= t.Date).AsEnumerable();
            TempData["f"] = f.ToShortDateString();
            TempData["t"] = t.ToShortDateString();
            return View(m);
        }

        [HttpPost]
        public async Task<JsonResult> MedAdd([FromBody] Addmini a)
        {
            var b = await _context.PrescriptionMedications.Where(x=>x.Id == a.Id).FirstOrDefaultAsync();
            if (b != null)
            {
                b.QuantityAdministered += a.Count;
                b.AdministeredDate = a.DateTime;
                _context.PrescriptionMedications.Update(b);
                await _context.SaveChangesAsync();
                return Json(true);
            }
            return Json(null);
        }

        [HttpPost]
        public async Task<IActionResult> SearchPatient(string? id)
        {
            return RedirectToAction(nameof(ViewPatient), new { id });
        }
        public async Task<IActionResult> SearchPatient()
        {
            return View();
        }
        public async Task<IActionResult> ReportDate()
        {
            return View();
        }

        public async Task<IActionResult> Administer(int? id, int? pat)
        {
            if (id != null && pat != null)
            {
                var m = await _context.PrescriptionRecords.Include(c => c.Patient).Where(x => x.PatientID == pat && x.PrescriptionID == id).FirstOrDefaultAsync();
                var i = _context.PrescriptionMedications.Where(x => x.PrescriptionID == m.PrescriptionID).Include(c=>c.MedicationRecords).Select(c=> new MedList
                {
                    Name = c.MedicationRecords.MedicationName,
                    qty = c.Quantity,
                    Inst = c.Instructions,
                    AdQty = c.QuantityAdministered,
                    id = c.Id,
                }).AsEnumerable();
                if (m != null && i != null)
                {
                    ViewPres v = new()
                    {
                        Pres = m,
                        Items = i,
                    };
                    m.PrescriptionStatus = "Received";
                    m.NurseID = HttpContext.Session.GetString("userid");
                    _context.PrescriptionRecords.Update(m);
                    await _context.SaveChangesAsync();
                    return View(v);
                }
            }
            TempData["error"] = "Record not found";
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Administers()
        {
            var po = _context.PrescriptionMedications.Include(c => c.PrescriptionRecords).Where(x => (x.PrescriptionRecords.PrescriptionStatus == "Received" || x.PrescriptionRecords.PrescriptionStatus == "Dispensed")).AsEnumerable();
            var pres = _context.PrescriptionRecords.Where(x => (x.PrescriptionStatus == "Received" || x.PrescriptionStatus == "Dispensed"));
            var admit1 = _context.PatientAdmissions.Include(c=>c.Patient).Include(c=>c.Ward).Include(c=>c.Bed).Where(x => x.Id == HttpContext.Session.GetString("userid"));
            


            var m = (from p in po
                     join a in admit1 on p.PrescriptionRecords.PatientID equals a.PatientID
                     select new ListPre
                     {
                         PatientId = p.PrescriptionRecords.PatientID,
                         PatIDNo = a.Patient.PatientIDNumber,
                         PresId = p.PrescriptionID,
                         PatSurname = a.Patient.Surname,
                         Ward = a.Ward.WardName,
                         Bed = a.Bed.BedNo,
                         Status = p.PrescriptionRecords.PrescriptionStatus,
                         PatName = p.PrescriptionRecords.Patient.Name,
                         Qty = p.Quantity,
                         QtyAd = p.QuantityAdministered
                     }).DistinctBy(c=>c.PresId).AsEnumerable();
            return View(m);
        }

        public async Task<IActionResult> RetakeVitals(string? id)
        {
            ViewBag.Vitals = _context.Vitals;
            ViewBag.Vital = _context.Vitals.ToList();
            return View(new { id });
        }
        [HttpGet]
        public async Task<JsonResult> SearchPatientData(string id)
        {
            var pat = await _context.Patients.Where(x=>x.PatientIDNumber == id).FirstOrDefaultAsync();
            if (pat != null)
            {
                return Json(pat);
            }
            return Json(null);
        }

        //NB: Important. I made changes to the Patient Vitals Model, can you please apply the changes based on your code on everything commented out
        [HttpGet]
        public async Task<JsonResult> GetVitals(int? id)
        {
            if (id != null)
            {
                var v = _context.Vitals.AsEnumerable();
                List<ViewPatVitals> list = new();
                foreach (var item in v)
                {
                    var p = new ViewPatVitals()
                    {
                        PatientVitals = _context.PatientVitals.Include(c => c.Vitals).Where(x => x.PatientID == id && x.VitalID == item.VitalID).AsEnumerable(),
                        Vitals = item
                    };
                    list.Add(p);
                }
                
                return Json(list);
            }
            
            return Json(null);
        }

        [HttpPost]
        public async Task<JsonResult> AddVital([FromBody] PatientVM pat)
        {
            
            var p = await _context.Patients.Where(x => x.PatientIDNumber == pat.PatientId).FirstOrDefaultAsync();
            if (p != null)
            {
                pat.PatientVitals = pat.PatientVitals.DistinctBy(c=>c.VitalID);
                foreach (var item in pat.PatientVitals)
                {
                    item.PatientID = p.PatientID;
                }
                await _context.PatientVitals.AddRangeAsync(pat.PatientVitals);
                await _context.SaveChangesAsync();
                var ad = await _context.PatientAdmissions.Include(c => c.SurgeryBooking).Include(c => c.SurgeryBooking.Surgeon).Include(c => c.SurgeryBooking.Anaesthesiologist).Where(c => c.PatientID == p.PatientID).OrderByDescending(c => c.AdmissionDate).FirstOrDefaultAsync();
                if (ad != null)
                {
                    var emailMessage = new MimeMessage();
                    emailMessage.From.Add(new MailboxAddress("Health Hive", "noreply@HealthHive.com"));
                    var emailAddresses = new List<InternetAddress>()
                    {
                        new MailboxAddress($"{ad.SurgeryBooking.Surgeon.Name} {ad.SurgeryBooking.Surgeon.Surname}", ad.SurgeryBooking.Surgeon.EmailAddress),
                        new MailboxAddress($"{ad.SurgeryBooking.Anaesthesiologist.Name} {ad.SurgeryBooking.Anaesthesiologist.Surname}", ad.SurgeryBooking.Anaesthesiologist.EmailAddress),
                    };
                    emailMessage.To.AddRange(emailAddresses);
                    emailMessage.Subject = $"GRP-04-16. {p.Name} {p.Surname} - Retaken Vitals";
                    var m = (from v in _context.Vitals.AsEnumerable()
                             join i in pat.PatientVitals on v.VitalID equals i.VitalID
                             select new
                             {
                                 name = v.Name,
                                 value = i.Value1,
                                 value2 = i.Value2,
                             }).AsEnumerable();
                    var bodyBuilder = new BodyBuilder
                    {
                        HtmlBody = $@"<p>Dear: {p.Name} {p.Surname} </p>
                                            <p><strong>Nurse Name:</strong> {HttpContext.Session.GetString("user_full_name")}</p>
                                            <p><strong>Patient ID Number:</strong> {p.PatientIDNumber}</p>
                                            <p><strong>Patient Name:</strong> {p.Name} {p.Surname}</p>
                                            <p><strong>Date: </strong> {pat.PatientVitals.FirstOrDefault().DateTime.ToShortDateString()}</p>
                                            
                                            <h3>Vitals</h3>
                                            <table >
                                            <thead>
                                                <tr></tr>
                                                <tr>Reading</tr>
                                                <tr></tr>
                                            </thead>
                                            <tbody>
                                               {string.Join("", m.Select(v => $@"
                                                       <tr>
                                                        <td>{v.name}</td>
                                                        <td>{v.value}</td>
                                                        <td>{(v.value2 == 0 ? "" : v.value2)}</td>
                                                    </tr>"))}
                                            </tbody>
                                        
                                        </table>
                                        <p>Nurse note: {pat.Type}</p>"
                    };

                    emailMessage.Body = bodyBuilder.ToMessageBody();

                    using (var client = new SmtpClient())
                    {
                        client.Connect("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
                        client.Authenticate("hivehealth628@gmail.com", "pcbbjzzjndrmarqi");
                        await client.SendAsync(emailMessage);
                        client.Disconnect(true);
                    }
                }
                
                return Json(new { r = true});
            }
            return Json(null);
        }

        [HttpPost]
        public async Task<IActionResult> SearchPatientBooking(string? id)
        {
            if (id == null)
            {
                TempData["error"] = "Id not found";
                return RedirectToAction(nameof(Index));
            }
            var applicationDbContext = _context.SurgeryBooking.Include(s => s.Anaesthesiologist).Include(s => s.Patient).Include(s => s.Surgeon).Include(s => s.Theatre).Where(x => x.Patient.PatientIDNumber == id && x.IsActive);
            return View("Bookings", await applicationDbContext.ToListAsync());
        }

        public async Task<IActionResult> AdmitPatient(int? id)
        {
            
            if (id == null)
            {
                TempData["error"] = "Record not found";
                return RedirectToAction("Index");
            }
            var su = await _context.SurgeryBooking.Where(x => x.SurgeryID == id ).Include(s => s.Patient).FirstOrDefaultAsync();
            if (su == null || su.Patient == null)
            {
                TempData["error"] = "Record not found";
                return RedirectToAction("Index");
            }
            var pat = await _context.Patients.Where(x=>x.PatientIDNumber == su.Patient.PatientIDNumber).FirstOrDefaultAsync();
            if (pat == null)
            {
                TempData["error"] = "Patient not Found";
                return RedirectToAction(nameof(Index));
            }
            ViewData["Ward"] = new SelectList(_context.WardRecords, "WardID", "WardName");
            ViewData["Bed"] = new SelectList(_context.BedRecords, "BedID", "BedNo");
            ViewData["ActiveIn"] = new SelectList(_context.ActiveIngredientRecords, "IngredientID", "IngredientName");
            ViewData["Conditions"] = new SelectList(_context.Condition, "ConditionID", "Diagnosis");
            ViewData["Medicines"] = new SelectList(_context.MedicationRecords, "MedicationID", "MedicationName");
            TempData["id"] = id;
            pat.Vitals = _context.Vitals;
            return View(pat);
        }
        [HttpPost]
        public async Task<JsonResult> AdmitPatient1([FromBody] AdmitPat pat)
        {
            if (ModelState.IsValid)
            {
                
                var p = _context.Patients.Where(x=>x.PatientIDNumber == pat.Patient.PatientIDNumber).FirstOrDefault();
                p.Name = pat.Patient.Name;
                p.Surname = pat.Patient.Surname;
                p.DateOfBirth = pat.Patient.DateOfBirth;
                p.Gender = pat.Patient.Gender;
                p.EmailAddress = pat.Patient.EmailAddress;
                p.ContactNumber = pat.Patient.ContactNumber;
                _context.Patients.Update(p);
                await _context.SaveChangesAsync();
                var ad = new PatientAdmission()
                {
                    PatientID = p.PatientID,
                    WardID = pat.Ward,
                    BedID = pat.Bed,
                    Id= HttpContext.Session.GetString("userid"),
                    Height = pat.Height,
                    Weight = pat.Weight,
                    SurgeryBID = int.Parse(pat.BookingID),
                    BMI = ((double)pat.Weight / (pat.Height * pat.Height)) * 10000,
                };
                await _context.PatientAdmissions.AddAsync(ad);
                await _context.SaveChangesAsync();
                if (pat.History.Conditions.Any())
                {
                    var l = new List<PatientCondition>();
                    foreach (var item in pat.History.Conditions)
                    {
                        var i = new PatientCondition()
                        {
                            ConditionID = item.Id,
                            PatientID = p.PatientID
                        };
                        l.Add(i);
                    }
                    await _context.PatientCondition.AddRangeAsync(l);
                    await _context.SaveChangesAsync();
                }
                if (pat.History.Allergies.Any())
                {
                    var l = new List<Allergies>();
                    foreach (var item in pat.History.Allergies)
                    {
                        var i = new Allergies()
                        {
                            IngredientID = item.Id,
                            PatientID = p.PatientID
                        };
                        l.Add(i);
                    }
                    await _context.Allergies.AddRangeAsync(l);
                    await _context.SaveChangesAsync();
                }
                if (pat.History.Medicines.Any())
                {
                    var l = new List<PatientMedication>();
                    foreach (var item in pat.History.Medicines)
                    {
                        var i = new PatientMedication()
                        {
                            MedicationID = item.Id,
                            PatientID = p.PatientID
                        };
                        l.Add(i);
                    }
                    await _context.PatientMedications.AddRangeAsync(l);
                    await _context.SaveChangesAsync();
                }
                pat.PatientVitals = pat.PatientVitals.DistinctBy(c => c.VitalID);
                foreach (var item in pat.PatientVitals)
                {
                    item.PatientID = p.PatientID;
                }
                await _context.PatientVitals.AddRangeAsync(pat.PatientVitals);
                await _context.SaveChangesAsync();
                var b = await _context.SurgeryBooking.Include(s => s.Patient).Where(x => x.SurgeryID == int.Parse(pat.BookingID)).FirstOrDefaultAsync();
                b.IsActive = false;
                _context.SurgeryBooking.UpdateRange(b);
                await _context.SaveChangesAsync();

                return Json(true);
            }
            return Json(false);
        }
        public async Task<IActionResult> Admitted()
        {
            var applicationDbContext = _context.PatientAdmissions.Include(p => p.Patient).Include(p => p.Ward).Include(p => p.Bed).Where(x=>x.Id == HttpContext.Session.GetString("userid") && x.DischargeDate == null); // w passing nurse ID as a string  same must be done to othr medicakpactitioners
            return View(await applicationDbContext.ToListAsync());
            //int.Parse(HttpContext.Session.GetString("id"))
        }
        public async Task<IActionResult> Discharge(int? id)
        {
            var a = await _context.PatientAdmissions.Where(x=>x.PatientAdmissionID == id && x.DischargeDate == null).FirstOrDefaultAsync();
            if (a!=null)
            {
                a.DischargeDate = DateTime.Now;
                _context.PatientAdmissions.Update(a);

                await _context.SaveChangesAsync();
                TempData["pass"] = "Patient has been discharged";
                return RedirectToAction(nameof(Index));
            }
            TempData["error"] = "Record not found";
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> ViewBooking(int? id)
        {
            if (id == null)
            {
                TempData["error"] = "Record not found";
                return RedirectToAction(nameof(Index));
            }

            var surgeryBooking = await _context.SurgeryBooking
                .Include(s => s.Anaesthesiologist)
                .Include(s => s.Patient)
                .Include(s => s.Surgeon)
                .Include(s => s.Theatre)
                .FirstOrDefaultAsync(m => m.SurgeryID == id);
            if (surgeryBooking == null)
            {
                TempData["error"] = "Record not found";
                return RedirectToAction(nameof(Index));
            }

            return View(surgeryBooking);
        }
        public async Task<IActionResult> ViewPatient(string? id)
        {
            if (id == null)
            {
                TempData["error"] = "Record not found";
                return RedirectToAction(nameof(Index));
            }
            var pat = await _context.Patients.Where(x => x.PatientIDNumber == id).FirstOrDefaultAsync();
            if (pat != null)

            {
                var medicalHistories = _context.MedicalHistories
                .Include(m => m.Patient)
                .Include(m => m.MedicalHistoryAllergies)
                    .ThenInclude(mha => mha.ActiveIngredientRecords)
                .Include(m => m.MedicalHistoryMedications)
                    .ThenInclude(mhm => mhm.MedicationRecords)
                .Include(m => m.MedicalHistoryCondition)
                    .ThenInclude(mhc => mhc.Condition).Where(c=>c.PatientID == pat.PatientID)
                .AsEnumerable();
                var con = _context.PatientCondition.Include(c=>c.Condition).Where(x=>x.PatientID == pat.PatientID).AsEnumerable().DistinctBy(x=>x.ConditionID);
                var all = _context.Allergies.Include(c => c.Ingredient).Where(x => x.PatientID == pat.PatientID).AsEnumerable().DistinctBy(x=>x.IngredientID);
                var med = _context.PatientMedications.Include(c=>c.Medication).Where(x => x.PatientID == pat.PatientID).AsEnumerable().DistinctBy(x=>x.MedicationID);

                var vit = _context.PatientVitals.Include(c=>c.Vitals).Where(c => c.PatientID == pat.PatientID).AsEnumerable();
                ViewPatient v = new()
                {
                    Patient = pat,
                    Vitals = vit,
                    Condition = con,
                    PatientMedication = med,
                    Allergies = all,
                    MedicalHistory = medicalHistories
                };
                ViewBag.Vitals = _context.Vitals;
                return View(v);
            }
            TempData["error"] = "Record not found";
            return RedirectToAction(nameof(Index));
        }
    }
}
