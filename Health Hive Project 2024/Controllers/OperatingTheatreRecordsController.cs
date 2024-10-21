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
    public class OperatingTheatreRecordsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OperatingTheatreRecordsController(ApplicationDbContext context)
        {
            _context = context;
        }


        [HttpGet]
        public IActionResult CreateMultipleRecords()
        {
            var model = new CreateRecordsViewModel
            {
                OperatingTheatre = new OperatingTheatreRecords(),
                Ward = new WardRecords(),
                Bed = new BedRecords()
            };

            // If you need to pass any data to the view, e.g., a list of wards
            ViewData["WardID"] = new SelectList(_context.WardRecords, "WardID", "WardName");

            return View("~/Views/OperatingTheatreRecords/CreateMultipleRecords.cshtml.cshtml", model);

        }

        [HttpPost]
        public IActionResult CreateMultipleRecords(CreateRecordsViewModel model)
        {
            
                // Save the records to the database
                _context.OperatingTheatreRecords.Add(model.OperatingTheatre);
                _context.WardRecords.Add(model.Ward);
                _context.BedRecords.Add(model.Bed);
                _context.SaveChanges();

                return RedirectToAction("Index");
            

            // Re-populate the dropdown if validation fails
            ViewData["WardID"] = new SelectList(_context.WardRecords, "WardID", "WardName");

            return View("~/Views/OperatingTheatreRecords/CreateMultipleRecords.cshtml.cshtml", model);
        }






        // GET: OperatingTheatreRecords
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.OperatingTheatreRecords;
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: OperatingTheatreRecords/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var operatingTheatreRecords = await _context.OperatingTheatreRecords
                .FirstOrDefaultAsync(m => m.TheatreID == id);
            if (operatingTheatreRecords == null)
            {
                return NotFound();
            }

            return View(operatingTheatreRecords);
        }

        // GET: OperatingTheatreRecords/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: OperatingTheatreRecords/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TheatreID,TheatreName,AdminID,MedicationID")] OperatingTheatreRecords operatingTheatreRecords)
        {
            if (ModelState.IsValid)
            {
                _context.Add(operatingTheatreRecords);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(operatingTheatreRecords);
        }

        // GET: OperatingTheatreRecords/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var operatingTheatreRecords = await _context.OperatingTheatreRecords.FindAsync(id);
            if (operatingTheatreRecords == null)
            {
                return NotFound();
            }
            return View(operatingTheatreRecords);
        }

        // POST: OperatingTheatreRecords/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TheatreID,TheatreName,AdminID,MedicationID")] OperatingTheatreRecords operatingTheatreRecords)
        {
            if (id != operatingTheatreRecords.TheatreID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(operatingTheatreRecords);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OperatingTheatreRecordsExists(operatingTheatreRecords.TheatreID))
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
            return View(operatingTheatreRecords);
        }

        // GET: OperatingTheatreRecords/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var operatingTheatreRecords = await _context.OperatingTheatreRecords
                .FirstOrDefaultAsync(m => m.TheatreID == id);
            if (operatingTheatreRecords == null)
            {
                return NotFound();
            }

            return View(operatingTheatreRecords);
        }

        // POST: OperatingTheatreRecords/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var operatingTheatreRecords = await _context.OperatingTheatreRecords.FindAsync(id);
            if (operatingTheatreRecords != null)
            {
                _context.OperatingTheatreRecords.Remove(operatingTheatreRecords);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OperatingTheatreRecordsExists(int id)
        {
            return _context.OperatingTheatreRecords.Any(e => e.TheatreID == id);
        }
    }
}
