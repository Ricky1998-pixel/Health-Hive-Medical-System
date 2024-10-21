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
    public class DailyStockReportsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DailyStockReportsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: DailyStockReports
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.DailyStockReports.Include(d => d.Medication);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: DailyStockReports/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dailyStockReport = await _context.DailyStockReports
                .Include(d => d.Medication)
                .FirstOrDefaultAsync(m => m.ReportID == id);
            if (dailyStockReport == null)
            {
                return NotFound();
            }

            return View(dailyStockReport);
        }

        // GET: DailyStockReports/Create
        public IActionResult Create()
        {
            ViewData["MedicationID"] = new SelectList(_context.MedicationRecords, "MedicationID", "MedicationName");
            return View();
        }

        // POST: DailyStockReports/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ReportID,MedicationID,QuantityUsed,DateUsed")] DailyStockReport dailyStockReport)
        {
            if (ModelState.IsValid)
            {
                _context.Add(dailyStockReport);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MedicationID"] = new SelectList(_context.MedicationRecords, "MedicationID", "MedicationName", dailyStockReport.MedicationID);
            return View(dailyStockReport);
        }

        // GET: DailyStockReports/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dailyStockReport = await _context.DailyStockReports.FindAsync(id);
            if (dailyStockReport == null)
            {
                return NotFound();
            }
            ViewData["MedicationID"] = new SelectList(_context.MedicationRecords, "MedicationID", "MedicationName", dailyStockReport.MedicationID);
            return View(dailyStockReport);
        }

        // POST: DailyStockReports/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ReportID,MedicationID,QuantityUsed,DateUsed")] DailyStockReport dailyStockReport)
        {
            if (id != dailyStockReport.ReportID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dailyStockReport);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DailyStockReportExists(dailyStockReport.ReportID))
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
            ViewData["MedicationID"] = new SelectList(_context.MedicationRecords, "MedicationID", "MedicationName", dailyStockReport.MedicationID);
            return View(dailyStockReport);
        }

        // GET: DailyStockReports/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dailyStockReport = await _context.DailyStockReports
                .Include(d => d.Medication)
                .FirstOrDefaultAsync(m => m.ReportID == id);
            if (dailyStockReport == null)
            {
                return NotFound();
            }

            return View(dailyStockReport);
        }

        // POST: DailyStockReports/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var dailyStockReport = await _context.DailyStockReports.FindAsync(id);
            if (dailyStockReport != null)
            {
                _context.DailyStockReports.Remove(dailyStockReport);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DailyStockReportExists(int id)
        {
            return _context.DailyStockReports.Any(e => e.ReportID == id);
        }
    }
}
