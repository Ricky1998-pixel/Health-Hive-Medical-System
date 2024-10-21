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
    public class PrescriptionItemsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PrescriptionItemsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: PrescriptionItems
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.PrescriptionItems.Include(p => p.Medication).Include(p => p.Prescription);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: PrescriptionItems/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var prescriptionItem = await _context.PrescriptionItems
                .Include(p => p.Medication)
                .Include(p => p.Prescription)
                .FirstOrDefaultAsync(m => m.PrescriptionItemID == id);
            if (prescriptionItem == null)
            {
                return NotFound();
            }

            return View(prescriptionItem);
        }

        // GET: PrescriptionItems/Create
        public IActionResult Create()
        {
            ViewData["MedicationID"] = new SelectList(_context.MedicationRecords, "MedicationID", "MedicationName");
            ViewData["PrescriptionID"] = new SelectList(_context.PrescriptionRecords, "PrescriptionID", "PrescriptionID");
            return View();
        }

        // POST: PrescriptionItems/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PrescriptionItemID,PrescriptionID,MedicationID,Quantity,Instructions")] PrescriptionItem prescriptionItem)
        {
            if (ModelState.IsValid)
            {
                _context.Add(prescriptionItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MedicationID"] = new SelectList(_context.MedicationRecords, "MedicationID", "MedicationName", prescriptionItem.MedicationID);
            ViewData["PrescriptionID"] = new SelectList(_context.PrescriptionRecords, "PrescriptionID", "PrescriptionID", prescriptionItem.PrescriptionID);
            return View(prescriptionItem);
        }

        // GET: PrescriptionItems/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var prescriptionItem = await _context.PrescriptionItems.FindAsync(id);
            if (prescriptionItem == null)
            {
                return NotFound();
            }
            ViewData["MedicationID"] = new SelectList(_context.MedicationRecords, "MedicationID", "MedicationName", prescriptionItem.MedicationID);
            ViewData["PrescriptionID"] = new SelectList(_context.PrescriptionRecords, "PrescriptionID", "PrescriptionID", prescriptionItem.PrescriptionID);
            return View(prescriptionItem);
        }

        // POST: PrescriptionItems/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PrescriptionItemID,PrescriptionID,MedicationID,Quantity,Instructions")] PrescriptionItem prescriptionItem)
        {
            if (id != prescriptionItem.PrescriptionItemID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(prescriptionItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PrescriptionItemExists(prescriptionItem.PrescriptionItemID))
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
            ViewData["MedicationID"] = new SelectList(_context.MedicationRecords, "MedicationID", "MedicationName", prescriptionItem.MedicationID);
            ViewData["PrescriptionID"] = new SelectList(_context.PrescriptionRecords, "PrescriptionID", "PrescriptionID", prescriptionItem.PrescriptionID);
            return View(prescriptionItem);
        }

        // GET: PrescriptionItems/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var prescriptionItem = await _context.PrescriptionItems
                .Include(p => p.Medication)
                .Include(p => p.Prescription)
                .FirstOrDefaultAsync(m => m.PrescriptionItemID == id);
            if (prescriptionItem == null)
            {
                return NotFound();
            }

            return View(prescriptionItem);
        }

        // POST: PrescriptionItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var prescriptionItem = await _context.PrescriptionItems.FindAsync(id);
            if (prescriptionItem != null)
            {
                _context.PrescriptionItems.Remove(prescriptionItem);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PrescriptionItemExists(int id)
        {
            return _context.PrescriptionItems.Any(e => e.PrescriptionItemID == id);
        }
    }
}
