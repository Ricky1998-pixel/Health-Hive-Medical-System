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
    public class DayHospitalsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DayHospitalsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: DayHospitals
        public async Task<IActionResult> Index()
        {
            return View(await _context.DayHospitals.ToListAsync());
        }

        // GET: DayHospitals/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dayHospital = await _context.DayHospitals
                .FirstOrDefaultAsync(m => m.HospitalID == id);
            if (dayHospital == null)
            {
                return NotFound();
            }

            return View(dayHospital);
        }

        // GET: DayHospitals/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: DayHospitals/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("HospitalID,HospitalName,Address,PostalCode,Suburb,City,Province,HospitalContactNumber,HospitalEmaiAddress,PracticeManage,PurchaseManagerEmailAddress")] DayHospital dayHospital)
        {
            if (ModelState.IsValid)
            {
                _context.Add(dayHospital);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(dayHospital);
        }

        // GET: DayHospitals/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dayHospital = await _context.DayHospitals.FindAsync(id);
            if (dayHospital == null)
            {
                return NotFound();
            }
            return View(dayHospital);
        }

        // POST: DayHospitals/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("HospitalID,HospitalName,Address,PostalCode,Suburb,City,Province,HospitalContactNumber,HospitalEmaiAddress,PracticeManage,PurchaseManagerEmailAddress")] DayHospital dayHospital)
        {
            if (id != dayHospital.HospitalID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dayHospital);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DayHospitalExists(dayHospital.HospitalID))
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
            return View(dayHospital);
        }

        // GET: DayHospitals/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dayHospital = await _context.DayHospitals
                .FirstOrDefaultAsync(m => m.HospitalID == id);
            if (dayHospital == null)
            {
                return NotFound();
            }

            return View(dayHospital);
        }

        // POST: DayHospitals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var dayHospital = await _context.DayHospitals.FindAsync(id);
            if (dayHospital != null)
            {
                _context.DayHospitals.Remove(dayHospital);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DayHospitalExists(int id)
        {
            return _context.DayHospitals.Any(e => e.HospitalID == id);
        }
    }
}
