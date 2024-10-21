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
    public class AnaesthesiaOrders1Controller : Controller
    {
        private readonly ApplicationDbContext _context;

        public AnaesthesiaOrders1Controller(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: AnaesthesiaOrders1
        public async Task<IActionResult> Index()
        {
            return View(await _context.AnaesthesiaOrder.ToListAsync());
        }

        // GET: AnaesthesiaOrders1/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var anaesthesiaOrder = await _context.AnaesthesiaOrder
                .FirstOrDefaultAsync(m => m.OrderID == id);
            if (anaesthesiaOrder == null)
            {
                return NotFound();
            }

            return View(anaesthesiaOrder);
        }

        // GET: AnaesthesiaOrders1/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: AnaesthesiaOrders1/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OrderID,Anaesthesiologist,Name,Date,MedicationtoOrder,Quantity,Notes,OrderStatus,OrderUrgency")] AnaesthesiaOrder anaesthesiaOrder)
        {
            if (ModelState.IsValid)
            {
                _context.Add(anaesthesiaOrder);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(anaesthesiaOrder);
        }

        // GET: AnaesthesiaOrders1/Edit/5
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
            return View(anaesthesiaOrder);
        }

        // POST: AnaesthesiaOrders1/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OrderID,Anaesthesiologist,Name,Date,MedicationtoOrder,Quantity,Notes,OrderStatus,OrderUrgency")] AnaesthesiaOrder anaesthesiaOrder)
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
            return View(anaesthesiaOrder);
        }

        // GET: AnaesthesiaOrders1/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var anaesthesiaOrder = await _context.AnaesthesiaOrder
                .FirstOrDefaultAsync(m => m.OrderID == id);
            if (anaesthesiaOrder == null)
            {
                return NotFound();
            }

            return View(anaesthesiaOrder);
        }

        // POST: AnaesthesiaOrders1/Delete/5
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
