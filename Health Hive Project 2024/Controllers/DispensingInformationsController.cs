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
    public class DispensingInformationsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DispensingInformationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: DispensingInformations
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.DispensingInformation.Include(d => d.Prescription);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: DispensingInformations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dispensingInformation = await _context.DispensingInformation
                .Include(d => d.Prescription)
                .FirstOrDefaultAsync(m => m.DispensingID == id);
            if (dispensingInformation == null)
            {
                return NotFound();
            }

            return View(dispensingInformation);
        }

        // GET: DispensingInformations/Create
        public IActionResult Create()
        {
            ViewData["PrescriptionID"] = new SelectList(_context.PrescriptionRecords, "PrescriptionID", "PrescriptionID");
            return View();
        }

        // POST: DispensingInformations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DispensingID,PrescriptionID,DispensingPharmacist,DispensingDateTime")] DispensingInformation dispensingInformation)
        {
            if (ModelState.IsValid)
            {
                _context.Add(dispensingInformation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PrescriptionID"] = new SelectList(_context.PrescriptionRecords, "PrescriptionID", "PrescriptionID", dispensingInformation.PrescriptionID);
            return View(dispensingInformation);
        }

        // GET: DispensingInformations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dispensingInformation = await _context.DispensingInformation.FindAsync(id);
            if (dispensingInformation == null)
            {
                return NotFound();
            }
            ViewData["PrescriptionID"] = new SelectList(_context.PrescriptionRecords, "PrescriptionID", "PrescriptionID", dispensingInformation.PrescriptionID);
            return View(dispensingInformation);
        }

        // POST: DispensingInformations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DispensingID,PrescriptionID,DispensingPharmacist,DispensingDateTime")] DispensingInformation dispensingInformation)
        {
            if (id != dispensingInformation.DispensingID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dispensingInformation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DispensingInformationExists(dispensingInformation.DispensingID))
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
            ViewData["PrescriptionID"] = new SelectList(_context.PrescriptionRecords, "PrescriptionID", "PrescriptionID", dispensingInformation.PrescriptionID);
            return View(dispensingInformation);
        }

        // GET: DispensingInformations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dispensingInformation = await _context.DispensingInformation
                .Include(d => d.Prescription)
                .FirstOrDefaultAsync(m => m.DispensingID == id);
            if (dispensingInformation == null)
            {
                return NotFound();
            }

            return View(dispensingInformation);
        }

        // POST: DispensingInformations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var dispensingInformation = await _context.DispensingInformation.FindAsync(id);
            if (dispensingInformation != null)
            {
                _context.DispensingInformation.Remove(dispensingInformation);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DispensingInformationExists(int id)
        {
            return _context.DispensingInformation.Any(e => e.DispensingID == id);
        }
    }
}
