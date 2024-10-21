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
    public class AnaesthesiaOrdersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AnaesthesiaOrdersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: AnaesthesiaOrders
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.AnaesthesiaOrder.Include(a => a.Anaesthesiologist).Include(a => a.Patient);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: AnaesthesiaOrders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var anaesthesiaOrder = await _context.AnaesthesiaOrder
                .Include(a => a.Anaesthesiologist)
                .Include(a => a.Patient)
                .FirstOrDefaultAsync(m => m.OrderID == id);
            if (anaesthesiaOrder == null)
            {
                return NotFound();
            }

            return View(anaesthesiaOrder);
        }

        //GET: AnaesthesiaOrders/Create
        public IActionResult Create()
        {
            ViewData["Id"] = new SelectList(_context.MedicalProfessionalRecords, "Id", "ContactNumber");
            ViewData["PatientID"] = new SelectList(_context.Patients, "PatientID", "Address");
            return View();
        }

        //POST: AnaesthesiaOrders/Create
        //To protect from overposting attacks, enable the specific properties you want to bind to.
        //For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

       [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OrderID,PatientID,Id")] AnaesthesiaOrder anaesthesiaOrder)
        {
            if (ModelState.IsValid)
            {
                _context.Add(anaesthesiaOrder);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Id"] = new SelectList(_context.MedicalProfessionalRecords, "MedicalProfessionalID", "ContactNumber", anaesthesiaOrder.Id);
            ViewData["PatientID"] = new SelectList(_context.Patients, "PatientID", "Address", anaesthesiaOrder.PatientId);
            return View(anaesthesiaOrder);
        }

        // GET: AnaesthesiaOrders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var anaesthesiaOrder = await _context.AnaesthesiaOrder.FindAsync(id);
            if (anaesthesiaOrder == null)
            {
                return NotFound();
            }
            ViewData["Id"] = new SelectList(_context.MedicalProfessionalRecords, "MedicalProfessionalID", "ContactNumber", anaesthesiaOrder.Id);
            ViewData["PatientID"] = new SelectList(_context.Patients, "PatientID", "Address", anaesthesiaOrder.PatientId);
            return View(anaesthesiaOrder);
        }

        //POST: AnaesthesiaOrders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OrderID,PatientID,Id")] AnaesthesiaOrder anaesthesiaOrder)
        {
            if (id != anaesthesiaOrder.OrderID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(anaesthesiaOrder);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AnaesthesiaOrderExists(anaesthesiaOrder.OrderID))
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
            ViewData["Id"] = new SelectList(_context.MedicalProfessionalRecords, "MedicalProfessionalID", "ContactNumber", anaesthesiaOrder.Id);
            ViewData["PatientID"] = new SelectList(_context.Patients, "PatientID", "Address", anaesthesiaOrder.PatientId);
            return View(anaesthesiaOrder);
        }

        // GET: AnaesthesiaOrders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var anaesthesiaOrder = await _context.AnaesthesiaOrder
                .Include(a => a.Anaesthesiologist)
                .Include(a => a.Patient)
                .FirstOrDefaultAsync(m => m.OrderID == id);
            if (anaesthesiaOrder == null)
            {
                return NotFound();
            }

            return View(anaesthesiaOrder);
        }

        // POST: AnaesthesiaOrders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var anaesthesiaOrder = await _context.AnaesthesiaOrder.FindAsync(id);
            if (anaesthesiaOrder != null)
            {
                _context.AnaesthesiaOrder.Remove(anaesthesiaOrder);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AnaesthesiaOrderExists(int id)
        {
            return _context.AnaesthesiaOrder.Any(e => e.OrderID == id);
        }
    }
}
