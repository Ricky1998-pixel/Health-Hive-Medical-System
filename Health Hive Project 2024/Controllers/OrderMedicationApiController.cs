using Health_Hive_Project_2024.Data;
using Health_Hive_Project_2024.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;


namespace Health_Hive_Project_2024.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderMedicationApiController : ControllerBase
    {
 
        private readonly ApplicationDbContext _context;

        public OrderMedicationApiController(ApplicationDbContext context)
        {
            _context = context;
        }


        // GET: api/OrderMedicationApi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderMedication>>> GetOrderMedications()
        {
            return await _context.OrderMedications
                .Include(o => o.MedicationOrders)
                .ToListAsync();
        }

        // GET: api/OrderMedicationApi/5
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderMedication>> GetOrderMedication(int id)
        {
            var orderMedication = await _context.OrderMedications
                .Include(o => o.MedicationOrders)
                .FirstOrDefaultAsync(o => o.OrderMedicationID == id);

            if (orderMedication == null)
            {
                return NotFound();
            }

            return orderMedication;
        }

        // POST: api/OrderMedicationApi
        [HttpPost]
        public async Task<ActionResult<OrderMedication>> PostOrderMedication(OrderMedication orderMedication)
        {
            orderMedication.OrderStatus = "Submitted"; // Automatically set status to "Submitted"

            _context.OrderMedications.Add(orderMedication);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetOrderMedication", new { id = orderMedication.OrderMedicationID }, orderMedication);
        }
    }
}
