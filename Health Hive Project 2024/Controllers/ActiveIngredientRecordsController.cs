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
    public class ActiveIngredientRecordsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ActiveIngredientRecordsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ActiveIngredientRecords
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.ActiveIngredientRecords;
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: ActiveIngredientRecords/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var activeIngredientRecords = await _context.ActiveIngredientRecords
                .FirstOrDefaultAsync(m => m.IngredientID == id);
            if (activeIngredientRecords == null)
            {
                return NotFound();
            }

            return View(activeIngredientRecords);
        }

        // GET: ActiveIngredientRecords/Create
        public IActionResult Create()
        {
            ViewData["MedicationID"] = new SelectList(_context.MedicationRecords, "MedicationID", "MedicationName");
            return View();
        }

        // POST: ActiveIngredientRecords/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("IngredientID,IngredientName,MedicationID")] ActiveIngredientRecords activeIngredientRecords)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(activeIngredientRecords);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(activeIngredientRecords);
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateMultiple(List<string> IngredientName)
        {
            if (IngredientName == null || !IngredientName.Any())
            {
                ModelState.AddModelError(string.Empty, "Please enter at least one ingredient.");
                return BadRequest(ModelState); // Return a bad request if no ingredients are provided
            }

            var addedIngredients = new List<string>(); // To store added ingredient names

            if (ModelState.IsValid)
            {
                foreach (var name in IngredientName)
                {
                    if (!string.IsNullOrEmpty(name))
                    {
                        var activeIngredientRecord = new ActiveIngredientRecords
                        {
                            IngredientName = name
                        };

                        _context.Add(activeIngredientRecord);
                        addedIngredients.Add(name); // Keep track of added ingredients
                    }
                }
                await _context.SaveChangesAsync();

                // Return a JSON response with a success message
                return Json(new { success = true, message = string.Join(", ", addedIngredients) });
            }

            return BadRequest(ModelState); // Return bad request if the model state is invalid
        }




        // GET: ActiveIngredientRecords/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var activeIngredientRecords = await _context.ActiveIngredientRecords.FindAsync(id);
            if (activeIngredientRecords == null)
            {
                return NotFound();
            }
            return View(activeIngredientRecords);
        }

        // POST: ActiveIngredientRecords/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IngredientID,IngredientName,MedicationID")] ActiveIngredientRecords activeIngredientRecords)
        {
            if (id != activeIngredientRecords.IngredientID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(activeIngredientRecords);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ActiveIngredientRecordsExists(activeIngredientRecords.IngredientID))
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
            return View(activeIngredientRecords);
        }

        // GET: ActiveIngredientRecords/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var activeIngredientRecords = await _context.ActiveIngredientRecords
                .FirstOrDefaultAsync(m => m.IngredientID == id);
            if (activeIngredientRecords == null)
            {
                return NotFound();
            }

            return View(activeIngredientRecords);
        }

        // POST: ActiveIngredientRecords/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var activeIngredientRecords = await _context.ActiveIngredientRecords.FindAsync(id);
            if (activeIngredientRecords != null)
            {
                _context.ActiveIngredientRecords.Remove(activeIngredientRecords);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ActiveIngredientRecordsExists(int id)
        {
            return _context.ActiveIngredientRecords.Any(e => e.IngredientID == id);
        }
    }
}
