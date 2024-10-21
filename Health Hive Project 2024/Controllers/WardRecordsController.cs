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
    public class WardRecordsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public WardRecordsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: WardRecords
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.WardRecords;
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: WardRecords/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var wardRecords = await _context.WardRecords
                .FirstOrDefaultAsync(m => m.WardID == id);
            if (wardRecords == null)
            {
                return NotFound();
            }

            return View(wardRecords);
        }

        // GET: WardRecords/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: WardRecords/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("WardID,WardName,Capacity,Location,WardType")] WardRecords wardRecords)
        {
            if (ModelState.IsValid)
            {
                _context.Add(wardRecords);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(wardRecords);
        }

        // GET: WardRecords/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var wardRecords = await _context.WardRecords.FindAsync(id);
            if (wardRecords == null)
            {
                return NotFound();
            }
            return View(wardRecords);
        }

        // POST: WardRecords/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("WardID,WardName,Capacity,Location,WardType,AdminID")] WardRecords wardRecords)
        {
            if (id != wardRecords.WardID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(wardRecords);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WardRecordsExists(wardRecords.WardID))
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
            return View(wardRecords);
        }

        // GET: WardRecords/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var wardRecords = await _context.WardRecords
                .FirstOrDefaultAsync(m => m.WardID == id);
            if (wardRecords == null)
            {
                return NotFound();
            }

            return View(wardRecords);
        }

        // POST: WardRecords/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var wardRecords = await _context.WardRecords.FindAsync(id);
            if (wardRecords != null)
            {
                _context.WardRecords.Remove(wardRecords);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WardRecordsExists(int id)
        {
            return _context.WardRecords.Any(e => e.WardID == id);
        }
    }
}
