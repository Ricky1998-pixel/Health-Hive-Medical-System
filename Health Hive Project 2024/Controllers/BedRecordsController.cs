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
    public class BedRecordsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BedRecordsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: BedRecords
        public async Task<IActionResult> Index()
        {
            return View(await _context.BedRecords.ToListAsync());
        }

        // GET: BedRecords/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bedRecords = await _context.BedRecords
                .FirstOrDefaultAsync(m => m.BedID == id);
            if (bedRecords == null)
            {
                return NotFound();
            }

            return View(bedRecords);
        }

        // GET: BedRecords/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: BedRecords/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BedID,BedNo,IsAvailable")] BedRecords bedRecords)
        {
            if (ModelState.IsValid)
            {
                _context.Add(bedRecords);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(bedRecords);
        }

        // GET: BedRecords/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bedRecords = await _context.BedRecords.FindAsync(id);
            if (bedRecords == null)
            {
                return NotFound();
            }
            return View(bedRecords);
        }

        // POST: BedRecords/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BedID,BedNo,IsAvailable")] BedRecords bedRecords)
        {
            if (id != bedRecords.BedID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(bedRecords);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BedRecordsExists(bedRecords.BedID))
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
            return View(bedRecords);
        }

        // GET: BedRecords/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bedRecords = await _context.BedRecords
                .FirstOrDefaultAsync(m => m.BedID == id);
            if (bedRecords == null)
            {
                return NotFound();
            }

            return View(bedRecords);
        }

        // POST: BedRecords/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var bedRecords = await _context.BedRecords.FindAsync(id);
            if (bedRecords != null)
            {
                _context.BedRecords.Remove(bedRecords);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BedRecordsExists(int id)
        {
            return _context.BedRecords.Any(e => e.BedID == id);
        }
    }
}
