using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Health_Hive_Project_2024.Data;
using Health_Hive_Project_2024.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace Health_Hive_Project_2024.Controllers
{
    [Authorize]
    public class AllergiesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly SignInManager<MedicalProfessionalRecords> _signInManager;

        public AllergiesController(ApplicationDbContext context, SignInManager<MedicalProfessionalRecords> _signInManager)
        {
            _context = context;
            this._signInManager = _signInManager;
        }
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }
        // GET: Allergies
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Allergies.Include(a => a.Ingredient).Include(a => a.Patient);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Allergies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var allergies = await _context.Allergies
                .Include(a => a.Ingredient)
                .Include(a => a.Patient)
                .FirstOrDefaultAsync(m => m.AllergyID == id);
            if (allergies == null)
            {
                return NotFound();
            }

            return View(allergies);
        }

        // GET: Allergies/Create
        public IActionResult Create()
        {
            ViewData["IngredientID"] = new SelectList(_context.ActiveIngredientRecords, "IngredientID", "IngredientName");
            ViewData["PatientID"] = new SelectList(_context.Patients, "PatientID", "Address");
            return View();
        }

        // POST: Allergies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AllergyID,PatientID,AllergyType,AllergyDescription,IngredientID")] Allergies allergies)
        {
            if (ModelState.IsValid)
            {
                _context.Add(allergies);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IngredientID"] = new SelectList(_context.ActiveIngredientRecords, "IngredientID", "IngredientName", allergies.IngredientID);
            ViewData["PatientID"] = new SelectList(_context.Patients, "PatientID", "Address", allergies.PatientID);
            return View(allergies);
        }

        // GET: Allergies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var allergies = await _context.Allergies.FindAsync(id);
            if (allergies == null)
            {
                return NotFound();
            }
            ViewData["IngredientID"] = new SelectList(_context.ActiveIngredientRecords, "IngredientID", "IngredientName", allergies.IngredientID);
            ViewData["PatientID"] = new SelectList(_context.Patients, "PatientID", "Address", allergies.PatientID);
            return View(allergies);
        }

        // POST: Allergies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AllergyID,PatientID,AllergyType,AllergyDescription,IngredientID")] Allergies allergies)
        {
            if (id != allergies.AllergyID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(allergies);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AllergiesExists(allergies.AllergyID))
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
            ViewData["IngredientID"] = new SelectList(_context.ActiveIngredientRecords, "IngredientID", "IngredientName", allergies.IngredientID);
            ViewData["PatientID"] = new SelectList(_context.Patients, "PatientID", "Address", allergies.PatientID);
            return View(allergies);
        }

        // GET: Allergies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var allergies = await _context.Allergies
                .Include(a => a.Ingredient)
                .Include(a => a.Patient)
                .FirstOrDefaultAsync(m => m.AllergyID == id);
            if (allergies == null)
            {
                return NotFound();
            }

            return View(allergies);
        }

        // POST: Allergies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var allergies = await _context.Allergies.FindAsync(id);
            if (allergies != null)
            {
                _context.Allergies.Remove(allergies);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AllergiesExists(int id)
        {
            return _context.Allergies.Any(e => e.AllergyID == id);
        }
    }
}
