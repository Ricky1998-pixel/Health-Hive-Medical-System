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
    public class VitalTypesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VitalTypesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: VitalTypes
        public async Task<IActionResult> Index()
        {
            return View(await _context.VitalType.ToListAsync());
        }

        // GET: VitalTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vitalType = await _context.VitalType
                .FirstOrDefaultAsync(m => m.VitalTypeID == id);
            if (vitalType == null)
            {
                return NotFound();
            }

            return View(vitalType);
        }

        // GET: VitalTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: VitalTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("VitalTypeID,BloodPressure,PulseRate,RespitoryRate,BloodOxygen,BloodGlucoseLevel")] VitalType vitalType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(vitalType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(vitalType);
        }

        // GET: VitalTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vitalType = await _context.VitalType.FindAsync(id);
            if (vitalType == null)
            {
                return NotFound();
            }
            return View(vitalType);
        }

        // POST: VitalTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("VitalTypeID,BloodPressure,PulseRate,RespitoryRate,BloodOxygen,BloodGlucoseLevel")] VitalType vitalType)
        {
            if (id != vitalType.VitalTypeID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(vitalType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VitalTypeExists(vitalType.VitalTypeID))
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
            return View(vitalType);
        }

        // GET: VitalTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vitalType = await _context.VitalType
                .FirstOrDefaultAsync(m => m.VitalTypeID == id);
            if (vitalType == null)
            {
                return NotFound();
            }

            return View(vitalType);
        }

        // POST: VitalTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var vitalType = await _context.VitalType.FindAsync(id);
            if (vitalType != null)
            {
                _context.VitalType.Remove(vitalType);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VitalTypeExists(int id)
        {
            return _context.VitalType.Any(e => e.VitalTypeID == id);
        }
    }
}
