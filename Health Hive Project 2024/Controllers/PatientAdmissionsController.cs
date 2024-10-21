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
    public class PatientAdmissionsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PatientAdmissionsController(ApplicationDbContext context)
        {
            _context = context;
        }


        
            // GET: PatientAdmissions/ViewAdmissions
            public async Task<IActionResult> ViewAdmissions()
            {
                var admittedPatients = await _context.PatientAdmissions
                    .Include(pa => pa.Patient)
                    .Include(pa => pa.Nurse)
                    .Include(pa => pa.Ward)
                    .Include(pa => pa.Bed)
                    .ToListAsync();

                return View(admittedPatients);
            }
        




        // GET: PatientAdmissions
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.PatientAdmissions.Include(p => p.Bed).Include(p => p.Nurse).Include(p => p.Patient).Include(p => p.Ward);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: PatientAdmissions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var patientAdmission = await _context.PatientAdmissions
                .Include(p => p.Bed)
                .Include(p => p.Nurse)
                .Include(p => p.Patient)
                .Include(p => p.Ward)
                .FirstOrDefaultAsync(m => m.PatientAdmissionID == id);
            if (patientAdmission == null)
            {
                return NotFound();
            }

            return View(patientAdmission);
        }

        // GET: PatientAdmissions/Create
        public IActionResult Create()
        {

            // Filter nurses based on their specialization
            var nurses = _context.MedicalProfessionalRecords
                .Where(m => m.Specialization == SpecializationType.Nurse)
                .Select(m => new { m.Id, FullName = m.Name + " " + m.Surname })
                .ToList(); // Ensure it's a list

            ViewData["BedID"] = new SelectList(_context.BedRecords, "BedID", "BedNo");
            ViewData["Id"] = new SelectList(nurses, "MedicalProfessionalID", "FullName");
            ViewData["PatientID"] = new SelectList(_context.Patients.Select(p => new { p.PatientID, FullName = p.Name + " " + p.Surname }).ToList(), "PatientID", "FullName");
            ViewData["WardID"] = new SelectList(_context.WardRecords, "WardID", "Location");
            return View();
        }

        // POST: PatientAdmissions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PatientAdmissionID,PatientID,AdmissionDate,DischargeDate,Id,WardID,BedID,Height,SurgeryBID,Weight")] PatientAdmission patientAdmission)
        {
            
                _context.Add(patientAdmission);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            
            ViewData["BedID"] = new SelectList(_context.BedRecords, "BedID", "BedNo", patientAdmission.BedID);
            //ViewData["Id"] = new SelectList(_context.MedicalProfessionalRecords, "MedicalProfessionalID", "ContactNumber", patientAdmission.Id);
            ViewData["Id"] = new SelectList(_context.MedicalProfessionalRecords
        .Where(m => m.Specialization == SpecializationType.Nurse)
        .Select(m => new { m.Id, FullName = m.Name + " " + m.Surname })
        .ToList(), "MedicalProfessionalID", "FullName", patientAdmission.Id);
            ViewData["PatientID"] = new SelectList(_context.Patients.Select(p => new { p.PatientID, FullName = p.Name + " " + p.Surname }).ToList(), "PatientID", "FullName", patientAdmission.PatientID);
            ViewData["WardID"] = new SelectList(_context.WardRecords, "WardID", "Location", patientAdmission.WardID);
            return View(patientAdmission);
        }

        // GET: PatientAdmissions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var patientAdmission = await _context.PatientAdmissions.FindAsync(id);
            if (patientAdmission == null)
            {
                return NotFound();
            }
            ViewData["BedID"] = new SelectList(_context.BedRecords, "BedID", "BedNo", patientAdmission.BedID);
            //ViewData["Id"] = new SelectList(_context.MedicalProfessionalRecords, "MedicalProfessionalID", "ContactNumber", patientAdmission.Id);
            ViewData["Id"] = new SelectList(_context.MedicalProfessionalRecords
        .Where(m => m.Specialization == SpecializationType.Nurse)
        .Select(m => new { m.Id, FullName = m.Name + " " + m.Surname })
        .ToList(), "MedicalProfessionalID", "FullName", patientAdmission.Id);
            ViewData["PatientID"] = new SelectList(_context.Patients.Select(p => new { p.PatientID, FullName = p.Name + " " + p.Surname }).ToList(), "PatientID", "FullName", patientAdmission.PatientID);
            ViewData["WardID"] = new SelectList(_context.WardRecords, "WardID", "Location", patientAdmission.WardID);
            return View(patientAdmission);
        }

        // POST: PatientAdmissions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PatientAdmissionID,PatientID,AdmissionDate,DischargeDate,Id,WardID,BedID,Height,SurgeryBID,Weight")] PatientAdmission patientAdmission)
        {
            if (id != patientAdmission.PatientAdmissionID)
            {
                return NotFound();
            }

            
                try
                {
                    _context.Update(patientAdmission);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PatientAdmissionExists(patientAdmission.PatientAdmissionID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            
            ViewData["BedID"] = new SelectList(_context.BedRecords, "BedID", "BedNo", patientAdmission.BedID);
            //ViewData["Id"] = new SelectList(_context.MedicalProfessionalRecords, "MedicalProfessionalID", "ContactNumber", patientAdmission.Id);
            ViewData["Id"] = new SelectList(_context.MedicalProfessionalRecords
        .Where(m => m.Specialization == SpecializationType.Nurse)
        .Select(m => new { m.  Id, FullName = m.Name + " " + m.Surname })
        .ToList(), "MedicalProfessionalID", "FullName", patientAdmission.Id);
            ViewData["PatientID"] = new SelectList(_context.Patients.Select(p => new { p.PatientID, FullName = p.Name + " " + p.Surname }).ToList(), "PatientID", "FullName", patientAdmission.PatientID);
            ViewData["WardID"] = new SelectList(_context.WardRecords, "WardID", "Location", patientAdmission.WardID);
            return View(patientAdmission);
        }

        // GET: PatientAdmissions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var patientAdmission = await _context.PatientAdmissions
                .Include(p => p.Bed)
                .Include(p => p.Nurse)
                .Include(p => p.Patient)
                .Include(p => p.Ward)
                .FirstOrDefaultAsync(m => m.PatientAdmissionID == id);
            if (patientAdmission == null)
            {
                return NotFound();
            }

            return View(patientAdmission);
        }

        // POST: PatientAdmissions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var patientAdmission = await _context.PatientAdmissions.FindAsync(id);
            if (patientAdmission != null)
            {
                _context.PatientAdmissions.Remove(patientAdmission);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PatientAdmissionExists(int id)
        {
            return _context.PatientAdmissions.Any(e => e.PatientAdmissionID == id);
        }
    }
}
