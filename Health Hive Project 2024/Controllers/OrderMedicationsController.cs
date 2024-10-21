using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Health_Hive_Project_2024.Data;
using Health_Hive_Project_2024.Models;
using System.Linq;
using System.Threading.Tasks;

namespace Health_Hive_Project_2024.Controllers
{
    public class OrderMedicationsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrderMedicationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: OrderMedications/Create
        //public IActionResult Create()
        //{
        //    return View();
        //}
        public IActionResult Create()
        {
            ViewBag.Medications = new List<string> { "Medication A", "Medication B", "Medication C" }; // Replace with actual data from your database if needed
            return View();
        }


        // POST: OrderMedications/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(OrderMedication orderMedication)
        {
          
                // Automatically set status to "Submitted"
                orderMedication.OrderStatus = "Submitted";

                _context.Add(orderMedication);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));

            ViewBag.Medications = new List<string> { "Medication A", "Medication B", "Medication C" }; // Re-populate the medications in case of validation failure
        
            return View(orderMedication);
        }

        // GET: OrderMedications/Index
        public async Task<IActionResult> Index()
        {
            var orderMedications = await _context.OrderMedications
                .Include(o => o.MedicationOrders)
                .ToListAsync();
            return View(orderMedications);
        }
    }
}
