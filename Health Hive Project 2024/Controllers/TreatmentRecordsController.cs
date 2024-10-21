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
    public class TreatmentRecordsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TreatmentRecordsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: TreatmentRecords
        public async Task<IActionResult> Index()
        {
            return View(await _context.TreatmentRecords.ToListAsync());
        }

        // GET: TreatmentRecords/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var treatmentRecords = await _context.TreatmentRecords
                .FirstOrDefaultAsync(m => m.TreatmentID == id);
            if (treatmentRecords == null)
            {
                return NotFound();
            }

            return View(treatmentRecords);
        }

        // GET: TreatmentRecords/Create
        //public IActionResult Create()
        //{
        //    return View();
        //}

        // POST: TreatmentRecords/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("TreatmentID,TreatmentCode,Description")] TreatmentRecords treatmentRecords)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(treatmentRecords);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(treatmentRecords);
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(List<string> TreatmentCodes, List<string> Descriptions)
        {
            if (ModelState.IsValid)
            {
                // Loop through the lists and save each treatment record
                for (int i = 0; i < TreatmentCodes.Count; i++)
                {
                    var treatmentRecord = new TreatmentRecords
                    {
                        TreatmentCode = TreatmentCodes[i],
                        Description = Descriptions[i]
                    };

                    // Save each treatment record to the database
                    _context.TreatmentRecords.Add(treatmentRecord);
                }

                _context.SaveChanges();

                // Redirect to the same Create action with a success message
                return RedirectToAction(nameof(Create), new { success = true });
            }

            return View();
        }

        // GET Create action to handle the success parameter
        public IActionResult Create(bool success = false)
        {
            ViewBag.Success = success;
            return View();
        }



        // GET: TreatmentRecords/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var treatmentRecords = await _context.TreatmentRecords.FindAsync(id);
            if (treatmentRecords == null)
            {
                return NotFound();
            }
            return View(treatmentRecords);
        }

        // POST: TreatmentRecords/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TreatmentID,TreatmentCode,Description")] TreatmentRecords treatmentRecords)
        {
            if (id != treatmentRecords.TreatmentID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(treatmentRecords);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TreatmentRecordsExists(treatmentRecords.TreatmentID))
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
            return View(treatmentRecords);
        }

        // GET: TreatmentRecords/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var treatmentRecords = await _context.TreatmentRecords
                .FirstOrDefaultAsync(m => m.TreatmentID == id);
            if (treatmentRecords == null)
            {
                return NotFound();
            }

            return View(treatmentRecords);
        }

        // POST: TreatmentRecords/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var treatmentRecords = await _context.TreatmentRecords.FindAsync(id);
            if (treatmentRecords != null)
            {
                _context.TreatmentRecords.Remove(treatmentRecords);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TreatmentRecordsExists(int id)
        {
            return _context.TreatmentRecords.Any(e => e.TreatmentID == id);
        }
    }
}
