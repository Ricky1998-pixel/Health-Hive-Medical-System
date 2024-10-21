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
    public class MedicationAlertsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MedicationAlertsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: MedicationAlerts
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.MedicationAlerts.Include(m => m.Ingredient);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: MedicationAlerts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medicationAlert = await _context.MedicationAlerts
                .Include(m => m.Ingredient)
                .FirstOrDefaultAsync(m => m.MedicationAlertID == id);
            if (medicationAlert == null)
            {
                return NotFound();
            }

            return View(medicationAlert);
        }

        // GET: MedicationAlerts/Create
        public IActionResult Create()
        {
            ViewData["IngredientID"] = new SelectList(_context.ActiveIngredientRecords, "IngredientID", "IngredientName");
            return View();
        }

        // POST: MedicationAlerts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MedicationAlertID,IngredientID")] MedicationAlert medicationAlert)
        {
            if (ModelState.IsValid)
            {
                _context.Add(medicationAlert);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IngredientID"] = new SelectList(_context.ActiveIngredientRecords, "IngredientID", "IngredientName", medicationAlert.IngredientID);
            return View(medicationAlert);
        }

        // GET: MedicationAlerts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medicationAlert = await _context.MedicationAlerts.FindAsync(id);
            if (medicationAlert == null)
            {
                return NotFound();
            }
            ViewData["IngredientID"] = new SelectList(_context.ActiveIngredientRecords, "IngredientID", "IngredientName", medicationAlert.IngredientID);
            return View(medicationAlert);
        }

        // POST: MedicationAlerts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MedicationAlertID,IngredientID")] MedicationAlert medicationAlert)
        {
            if (id != medicationAlert.MedicationAlertID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(medicationAlert);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MedicationAlertExists(medicationAlert.MedicationAlertID))
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
            ViewData["IngredientID"] = new SelectList(_context.ActiveIngredientRecords, "IngredientID", "IngredientName", medicationAlert.IngredientID);
            return View(medicationAlert);
        }

        // GET: MedicationAlerts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medicationAlert = await _context.MedicationAlerts
                .Include(m => m.Ingredient)
                .FirstOrDefaultAsync(m => m.MedicationAlertID == id);
            if (medicationAlert == null)
            {
                return NotFound();
            }

            return View(medicationAlert);
        }

        // POST: MedicationAlerts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var medicationAlert = await _context.MedicationAlerts.FindAsync(id);
            if (medicationAlert != null)
            {
                _context.MedicationAlerts.Remove(medicationAlert);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MedicationAlertExists(int id)
        {
            return _context.MedicationAlerts.Any(e => e.MedicationAlertID == id);
        }
    }
}
