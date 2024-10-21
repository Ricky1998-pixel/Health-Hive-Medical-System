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
    public class OrderItemsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrderItemsController(ApplicationDbContext context)
        {
            _context = context;
        }





        // GET: OrderItems
        //public async Task<IActionResult> Index()
        //{
        //    var applicationDbContext = _context.OrderItem.Include(o => o.Medication);
        //    return View(await applicationDbContext.ToListAsync());
        //}


        //public async Task<IActionResult> Index(bool success = false)
        //{
        //    // Fetch the order items from the database including the Medication details
        //    var orderItems = await _context.OrderItem.Include(o => o.Medication).ToListAsync();

        //    // Pass the order items and success flag to the view
        //    ViewBag.Success = success;
        //    return View(orderItems);
        //}
        public async Task<IActionResult> Index(string searchString = "", bool success = false)
        {
            // Fetch the order items from the database including the Medication details
            var orderItems = from o in _context.OrderItem.Include(o => o.Medication)
                             select o;

            // Filter the order items based on the search string
            if (!string.IsNullOrEmpty(searchString))
            {
                orderItems = orderItems.Where(o => o.Medication.MedicationName.Contains(searchString));
            }

            // Convert to a list and pass to the view
            var orderItemsList = await orderItems.ToListAsync();

            // Pass the order items and success flag to the view
            ViewBag.Success = success;
            ViewBag.SearchString = searchString; // Pass the search string back to the view
            return View(orderItemsList);
        }










        // GET: OrderItems/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderItem = await _context.OrderItem
                .Include(o => o.Medication)
                .FirstOrDefaultAsync(m => m.ItemID == id);
            if (orderItem == null)
            {
                return NotFound();
            }

            return View(orderItem);
        }

        // GET: OrderItems/Create
        public IActionResult Create()
        {
            ViewData["MedicationID"] = new SelectList(_context.MedicationRecords, "MedicationID", "MedicationID");
            return View();
        }

        // POST: OrderItems/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("ItemID,MedicationID,Quantity")] OrderItem orderItem)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(orderItem);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["MedicationID"] = new SelectList(_context.MedicationRecords, "MedicationID", "MedicationID", orderItem.MedicationID);
        //    return View(orderItem);
        //}
        // POST: OrderItems/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ItemID,MedicationID,Quantity,IsUrgent")] OrderItem orderItem)
        {
            if (ModelState.IsValid)
            {
                _context.Add(orderItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MedicationID"] = new SelectList(_context.MedicationRecords, "MedicationID", "MedicationID", orderItem.MedicationID);
            return View(orderItem);
        }


        // GET: OrderItems/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderItem = await _context.OrderItem.FindAsync(id);
            if (orderItem == null)
            {
                return NotFound();
            }
            ViewData["MedicationID"] = new SelectList(_context.MedicationRecords, "MedicationID", "MedicationID", orderItem.MedicationID);
            return View(orderItem);
        }

        // POST: OrderItems/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("ItemID,MedicationID,Quantity")] OrderItem orderItem)
        //{
        //    if (id != orderItem.ItemID)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(orderItem);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!OrderItemExists(orderItem.ItemID))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["MedicationID"] = new SelectList(_context.MedicationRecords, "MedicationID", "MedicationID", orderItem.MedicationID);
        //    return View(orderItem);
        //}
        // POST: OrderItems/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ItemID,MedicationID,Quantity,IsUrgent")] OrderItem orderItem)
        {
            if (id != orderItem.ItemID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(orderItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderItemExists(orderItem.ItemID))
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
            ViewData["MedicationID"] = new SelectList(_context.MedicationRecords, "MedicationID", "MedicationID", orderItem.MedicationID);
            return View(orderItem);
        }


        // GET: OrderItems/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderItem = await _context.OrderItem
                .Include(o => o.Medication)
                .FirstOrDefaultAsync(m => m.ItemID == id);
            if (orderItem == null)
            {
                return NotFound();
            }

            return View(orderItem);
        }

        // POST: OrderItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var orderItem = await _context.OrderItem.FindAsync(id);
            if (orderItem != null)
            {
                _context.OrderItem.Remove(orderItem);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderItemExists(int id)
        {
            return _context.OrderItem.Any(e => e.ItemID == id);
        }
    }
}
