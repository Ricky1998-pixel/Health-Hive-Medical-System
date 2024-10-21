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
    public class MedicationIngredientsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MedicationIngredientsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: MedicationIngredients
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.MedicationIngredients.Include(m => m.Ingredient).Include(m => m.Medication);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: MedicationIngredients/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medicationIngredient = await _context.MedicationIngredients
                .Include(m => m.Ingredient)
                .Include(m => m.Medication)
                .FirstOrDefaultAsync(m => m.MedicationIngredientID == id);
            if (medicationIngredient == null)
            {
                return NotFound();
            }

            return View(medicationIngredient);
        }

        // GET: MedicationIngredients/Create
        public IActionResult Create()
        {
            ViewData["IngredientID"] = new SelectList(_context.ActiveIngredientRecords, "IngredientID", "IngredientName");
            ViewData["MedicationID"] = new SelectList(_context.MedicationRecords, "MedicationID", "MedicationName");
            return View();
        }

        // POST: MedicationIngredients/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MedicationIngredientID,MedicationID,IngredientID,ActiveIngredientStrength")] MedicationIngredient medicationIngredient)
        {
            
                _context.Add(medicationIngredient);
                await _context.SaveChangesAsync();
                return Json(new { success = true });

            // Repopulate dropdown lists in case of validation errors
            ViewData["IngredientID"] = new SelectList(_context.ActiveIngredientRecords, "IngredientID", "IngredientName", medicationIngredient.IngredientID);
            ViewData["MedicationID"] = new SelectList(_context.MedicationRecords, "MedicationID", "MedicationName", medicationIngredient.MedicationID);
            return Json(new { success = false, errors = ModelState.Values.SelectMany(v => v.Errors) });
        }


        // GET: MedicationIngredients/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medicationIngredient = await _context.MedicationIngredients.FindAsync(id);
            if (medicationIngredient == null)
            {
                return NotFound();
            }
            ViewData["IngredientID"] = new SelectList(_context.ActiveIngredientRecords, "IngredientID", "IngredientName", medicationIngredient.IngredientID);
            ViewData["MedicationID"] = new SelectList(_context.MedicationRecords, "MedicationID", "MedicationName", medicationIngredient.MedicationID);
            return View(medicationIngredient);
        }

        // POST: MedicationIngredients/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MedicationIngredientID,MedicationID,IngredientID,ActiveIngredientStrength")] MedicationIngredient medicationIngredient)
        {
            if (id != medicationIngredient.MedicationIngredientID)
            {
                return NotFound();
            }

           
            
                try
                {
                    _context.Update(medicationIngredient);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MedicationIngredientExists(medicationIngredient.MedicationIngredientID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            
            ViewData["IngredientID"] = new SelectList(_context.ActiveIngredientRecords, "IngredientID", "IngredientName", medicationIngredient.IngredientID);
            ViewData["MedicationID"] = new SelectList(_context.MedicationRecords, "MedicationID", "MedicationName", medicationIngredient.MedicationID);
            return View(medicationIngredient);
        }

        // GET: MedicationIngredients/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medicationIngredient = await _context.MedicationIngredients
                .Include(m => m.Ingredient)
                .Include(m => m.Medication)
                .FirstOrDefaultAsync(m => m.MedicationIngredientID == id);
            if (medicationIngredient == null)
            {
                return NotFound();
            }

            return View(medicationIngredient);
        }

        // POST: MedicationIngredients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var medicationIngredient = await _context.MedicationIngredients.FindAsync(id);
            if (medicationIngredient != null)
            {
                _context.MedicationIngredients.Remove(medicationIngredient);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MedicationIngredientExists(int id)
        {
            return _context.MedicationIngredients.Any(e => e.MedicationIngredientID == id);
        }
    }
}
