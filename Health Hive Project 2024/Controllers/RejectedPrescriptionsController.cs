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
    public class RejectedPrescriptionsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RejectedPrescriptionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        //// GET: RejectedPrescriptions
        //public async Task<IActionResult> Index()
        //{
        //    return View(await _context.RejectedPrescriptions.ToListAsync());
        //}

        // GET: RejectedPrescriptions
        public async Task<IActionResult> Index()
        {
            var rejectedPrescriptions = await _context.RejectedPrescriptions
                .Include(r => r.Surgeon)       // Load the related surgeon
                .Include(r => r.Pharmacist)
                .Include(r=> r.Patient)// Load the related pharmacist
                .Include(r => r.Medication)    // Load the related medication
                .ToListAsync();

            return View(rejectedPrescriptions);
        }


        // GET: RejectedPrescriptions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rejectedPrescription = await _context.RejectedPrescriptions
                .FirstOrDefaultAsync(m => m.RejectedPrescriptionID == id);
            if (rejectedPrescription == null)
            {
                return NotFound();
            }

            return View(rejectedPrescription);
        }

        // GET: RejectedPrescriptions/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: RejectedPrescriptions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RejectedPrescriptionID,PrescriptionID,SurgeonID,PatientID,Date,PrescriptionStatus,PharmacistID,MedicationID,Quantity,Instructions,RejectionReason,RejectedOn")] RejectedPrescription rejectedPrescription)
        {
            if (ModelState.IsValid)
            {
                _context.Add(rejectedPrescription);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(rejectedPrescription);
        }

        // GET: RejectedPrescriptions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rejectedPrescription = await _context.RejectedPrescriptions.FindAsync(id);
            if (rejectedPrescription == null)
            {
                return NotFound();
            }
            return View(rejectedPrescription);
        }

        // POST: RejectedPrescriptions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RejectedPrescriptionID,PrescriptionID,SurgeonID,PatientID,Date,PrescriptionStatus,PharmacistID,MedicationID,Quantity,Instructions,RejectionReason,RejectedOn")] RejectedPrescription rejectedPrescription)
        {
            if (id != rejectedPrescription.RejectedPrescriptionID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(rejectedPrescription);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RejectedPrescriptionExists(rejectedPrescription.RejectedPrescriptionID))
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
            return View(rejectedPrescription);
        }

        // GET: RejectedPrescriptions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rejectedPrescription = await _context.RejectedPrescriptions
                .FirstOrDefaultAsync(m => m.RejectedPrescriptionID == id);
            if (rejectedPrescription == null)
            {
                return NotFound();
            }

            return View(rejectedPrescription);
        }

        // POST: RejectedPrescriptions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var rejectedPrescription = await _context.RejectedPrescriptions.FindAsync(id);
            if (rejectedPrescription != null)
            {
                _context.RejectedPrescriptions.Remove(rejectedPrescription);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RejectedPrescriptionExists(int id)
        {
            return _context.RejectedPrescriptions.Any(e => e.RejectedPrescriptionID == id);
        }
    }
}
