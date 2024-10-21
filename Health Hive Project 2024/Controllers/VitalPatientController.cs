using Health_Hive_Project_2024.Data;
using Health_Hive_Project_2024.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Health_Hive_Project_2024.Controllers
{
    public class VitalPatientController : Controller
    {

        private readonly ApplicationDbContext _context;
        private readonly UserManager<MedicalProfessionalRecords> _userManager;
        private readonly IEmailService _emailService;
        //private readonly UserManager<IdentityUser> _userManager;
        public VitalPatientController(ApplicationDbContext context, UserManager<MedicalProfessionalRecords> userManager, IEmailService emailService)
        {
            _context = context;
            _userManager = userManager;
            _userManager = userManager;
            _emailService = emailService;
        }

        // GET: VitalPatient
        public async Task<IActionResult> Index()
        {
            var vitalPatients = await _context.Vitals
                .ToListAsync();
            return View(vitalPatients);
        }

        // GET: VitalPatient/Create
        public IActionResult Create()
        {

            return View();
        }
        [HttpPost]
        public IActionResult AddVital(Vitals model)
        {
            if (ModelState.IsValid)
            {
                // Add logic to save the new vital to the database
                _context.Vitals.Add(model);
                _context.SaveChanges();
                    return RedirectToAction("Index"); // or wherever the table is displayed
            }
            return View(model); // If validation fails, return the form
        }

        // GET: VitalPatient/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var vital = await _context.Vitals.FindAsync(id);
            if (vital == null) return NotFound();
            return PartialView("_EditModal", vital); // Return partial view
        }

        // POST: VitalPatient/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Vitals model)
        {
            if (ModelState.IsValid)
            {
                var existingVital = await _context.Vitals.FindAsync(model.VitalID);
                if (existingVital == null)
                {
                    return Json(new { success = false, message = "Vital not found." });
                }

                // Update the properties
                existingVital.Name = model.Name;
                existingVital.VitalName = model.VitalName;
                existingVital.VitalName2 = model.VitalName2;
                existingVital.MinimumValue = model.MinimumValue;
                existingVital.MaxValue = model.MaxValue;
              
                existingVital.UnitOfMeasure = model.UnitOfMeasure;
                _context.Update(existingVital);  // This will ensure EF tracks changes correctly
                await _context.SaveChangesAsync();

                return Json(new { success = true }); // Indicate success
            }

            return PartialView("_EditModal", model); // Return the modal with validation errors
        }


        // GET: VitalPatient/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var vital = await _context.Vitals.FindAsync(id);
            if (vital == null) return NotFound();
            return PartialView("_DeleteModal", vital); // Return partial view
        }

        // POST: VitalPatient/DeleteConfirmed/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var vital = await _context.Vitals.FindAsync(id);
            if (vital != null)
            {
                _context.Vitals.Remove(vital);
                await _context.SaveChangesAsync();
                return Json(new { success = true }); // Return success status
            }
            return Json(new { success = false, message = "Item not found." });
        }
    }
}

//        // GET: VitalPatient/Edit/5
//        public async Task<IActionResult> Edit(int id)
//        {
//            var vitalPatient = await _context.VitalPatients
//                .Include(vp => vp.Vitals)
//                .FirstOrDefaultAsync(vp => vp.VitalPatientId == id);

//            if (vitalPatient == null)
//            {
//                return NotFound();
//            }

//            ViewBag.VitalNames = _context.PatientVs.Select(v => v.VitalName).Distinct().ToList();
//            return View(vitalPatient);
//        }

//        // POST: VitalPatient/Edit/5
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Edit(int id, VitalPatient vitalPatient, List<PatientV> vitals)
//        {
//            if (id != vitalPatient.VitalPatientId)
//            {
//                return BadRequest();
//            }

//            if (ModelState.IsValid)
//            {
//                try
//                {
//                    _context.Update(vitalPatient);

//                    var existingVitals = _context.PatientVs.Where(v => v.VitalPatientId == id).ToList();
//                    _context.PatientVs.RemoveRange(existingVitals);

//                    foreach (var vital in vitals)
//                    {
//                        vital.VitalPatientId = vitalPatient.VitalPatientId;
//                        _context.PatientVs.Add(vital);
//                    }

//                    await _context.SaveChangesAsync();
//                }
//                catch (DbUpdateConcurrencyException)
//                {
//                    if (!VitalPatientExists(id))
//                    {
//                        return NotFound();
//                    }
//                    else
//                    {
//                        throw;
//                    }
//                }
//                return RedirectToAction(nameof(Index));
//            }

//            ViewBag.VitalNames = _context.PatientVs.Select(v => v.VitalName).Distinct().ToList();
//            return View(vitalPatient);
//        }

//        // GET: VitalPatient/Delete/5
//        public async Task<IActionResult> Delete(int id)
//        {
//            var vitalPatient = await _context.VitalPatients
//                .Include(vp => vp.Vitals)
//                .FirstOrDefaultAsync(vp => vp.VitalPatientId == id);

//            if (vitalPatient == null)
//            {
//                return NotFound();
//            }

//            return View(vitalPatient);
//        }

//        // POST: VitalPatient/Delete/5
//        [HttpPost, ActionName("Delete")]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> DeleteConfirmed(int id)
//        {
//            var vitalPatient = await _context.VitalPatients.FindAsync(id);
//            if (vitalPatient != null)
//            {
//                _context.VitalPatients.Remove(vitalPatient);
//                var vitals = _context.PatientVs.Where(v => v.VitalPatientId == id).ToList();
//                _context.PatientVs.RemoveRange(vitals);
//                await _context.SaveChangesAsync();
//            }
//            return RedirectToAction(nameof(Index));
//        }

//        private bool VitalPatientExists(int id)
//        {
//            return _context.VitalPatients.Any(e => e.VitalPatientId == id);
//        }
//    }
//}
