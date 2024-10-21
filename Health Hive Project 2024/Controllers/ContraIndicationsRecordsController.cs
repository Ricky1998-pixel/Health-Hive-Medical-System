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
    public class ContraIndicationsRecordsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ContraIndicationsRecordsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ContraIndicationsRecords
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.ContraIndicationsRecords.Include(c => c.Condition).Include(c => c.Ingredient);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: ContraIndicationsRecords/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contraIndicationsRecords = await _context.ContraIndicationsRecords
                .Include(c => c.Condition)
                .Include(c => c.Ingredient)
                .FirstOrDefaultAsync(m => m.ContraIndicationID == id);
            if (contraIndicationsRecords == null)
            {
                return NotFound();
            }

            return View(contraIndicationsRecords);
        }

        // GET: ContraIndicationsRecords/Create
        public IActionResult Create()
        {
            ViewData["ConditionID"] = new SelectList(_context.Condition, "ConditionID", "ConditionID");
            ViewData["IngredientID"] = new SelectList(_context.ActiveIngredientRecords, "IngredientID", "IngredientName");
            return View();
        }

        // POST: ContraIndicationsRecords/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ContraIndicationID,ConditionID,IngredientID,AlertMessage,AlertType")] ContraIndicationsRecords contraIndicationsRecords)
        {
            if (ModelState.IsValid)
            {
                _context.Add(contraIndicationsRecords);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ConditionID"] = new SelectList(_context.Condition, "ConditionID", "ConditionID", contraIndicationsRecords.ConditionID);
            ViewData["IngredientID"] = new SelectList(_context.ActiveIngredientRecords, "IngredientID", "IngredientName", contraIndicationsRecords.IngredientID);
            return View(contraIndicationsRecords);
        }

        // GET: ContraIndicationsRecords/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contraIndicationsRecords = await _context.ContraIndicationsRecords.FindAsync(id);
            if (contraIndicationsRecords == null)
            {
                return NotFound();
            }
            ViewData["ConditionID"] = new SelectList(_context.Condition, "ConditionID", "ConditionID", contraIndicationsRecords.ConditionID);
            ViewData["IngredientID"] = new SelectList(_context.ActiveIngredientRecords, "IngredientID", "IngredientName", contraIndicationsRecords.IngredientID);
            return View(contraIndicationsRecords);
        }

        // POST: ContraIndicationsRecords/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ContraIndicationID,ConditionID,IngredientID,AlertMessage,AlertType")] ContraIndicationsRecords contraIndicationsRecords)
        {
            if (id != contraIndicationsRecords.ContraIndicationID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(contraIndicationsRecords);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContraIndicationsRecordsExists(contraIndicationsRecords.ContraIndicationID))
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
            ViewData["ConditionID"] = new SelectList(_context.Condition, "ConditionID", "ConditionID", contraIndicationsRecords.ConditionID);
            ViewData["IngredientID"] = new SelectList(_context.ActiveIngredientRecords, "IngredientID", "IngredientName", contraIndicationsRecords.IngredientID);
            return View(contraIndicationsRecords);
        }

        // GET: ContraIndicationsRecords/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contraIndicationsRecords = await _context.ContraIndicationsRecords
                .Include(c => c.Condition)
                .Include(c => c.Ingredient)
                .FirstOrDefaultAsync(m => m.ContraIndicationID == id);
            if (contraIndicationsRecords == null)
            {
                return NotFound();
            }

            return View(contraIndicationsRecords);
        }

        // POST: ContraIndicationsRecords/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var contraIndicationsRecords = await _context.ContraIndicationsRecords.FindAsync(id);
            if (contraIndicationsRecords != null)
            {
                _context.ContraIndicationsRecords.Remove(contraIndicationsRecords);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ContraIndicationsRecordsExists(int id)
        {
            return _context.ContraIndicationsRecords.Any(e => e.ContraIndicationID == id);
        }
    }
}
