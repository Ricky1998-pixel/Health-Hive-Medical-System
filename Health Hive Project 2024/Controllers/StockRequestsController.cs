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
    public class StockRequestsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StockRequestsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: StockRequests
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.StockRequests.Include(s => s.Medication).Include(s => s.Pharmacist);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: StockRequests/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stockRequest = await _context.StockRequests
                .Include(s => s.Medication)
                .Include(s => s.Pharmacist)
                .FirstOrDefaultAsync(m => m.RequestID == id);
            if (stockRequest == null)
            {
                return NotFound();
            }

            return View(stockRequest);
        }

        // GET: StockRequests/Create
        public IActionResult Create()
        {
            ViewData["MedicationID"] = new SelectList(_context.MedicationRecords, "MedicationID", "MedicationName");
            ViewData["PharmacistID"] = new SelectList(_context.MedicalProfessionalRecords, "MedicalProfessionalID", "ContactNumber");
            return View();
        }

        // POST: StockRequests/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RequestID,PharmacistID,Quantity,RequestDate,Status,MedicationID,FulfillmentDate")] StockRequest stockRequest)
        {
            if (ModelState.IsValid)
            {
                _context.Add(stockRequest);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MedicationID"] = new SelectList(_context.MedicationRecords, "MedicationID", "MedicationName", stockRequest.MedicationID);
            ViewData["PharmacistID"] = new SelectList(_context.MedicalProfessionalRecords, "MedicalProfessionalID", "ContactNumber", stockRequest.PharmacistID);
            return View(stockRequest);
        }

        // GET: StockRequests/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stockRequest = await _context.StockRequests.FindAsync(id);
            if (stockRequest == null)
            {
                return NotFound();
            }
            ViewData["MedicationID"] = new SelectList(_context.MedicationRecords, "MedicationID", "MedicationName", stockRequest.MedicationID);
            ViewData["PharmacistID"] = new SelectList(_context.MedicalProfessionalRecords, "MedicalProfessionalID", "ContactNumber", stockRequest.PharmacistID);
            return View(stockRequest);
        }

        // POST: StockRequests/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RequestID,PharmacistID,Quantity,RequestDate,Status,MedicationID,FulfillmentDate")] StockRequest stockRequest)
        {
            if (id != stockRequest.RequestID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(stockRequest);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StockRequestExists(stockRequest.RequestID))
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
            ViewData["MedicationID"] = new SelectList(_context.MedicationRecords, "MedicationID", "MedicationName", stockRequest.MedicationID);
            ViewData["PharmacistID"] = new SelectList(_context.MedicalProfessionalRecords, "MedicalProfessionalID", "ContactNumber", stockRequest.PharmacistID);
            return View(stockRequest);
        }

        // GET: StockRequests/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stockRequest = await _context.StockRequests
                .Include(s => s.Medication)
                .Include(s => s.Pharmacist)
                .FirstOrDefaultAsync(m => m.RequestID == id);
            if (stockRequest == null)
            {
                return NotFound();
            }

            return View(stockRequest);
        }

        // POST: StockRequests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var stockRequest = await _context.StockRequests.FindAsync(id);
            if (stockRequest != null)
            {
                _context.StockRequests.Remove(stockRequest);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StockRequestExists(int id)
        {
            return _context.StockRequests.Any(e => e.RequestID == id);
        }
    }
}
