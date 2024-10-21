using Health_Hive_Project_2024.Data;
using Health_Hive_Project_2024.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Health_Hive_Project_2024.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VitalPatientApiController : ControllerBase
    {
  
        private readonly ApplicationDbContext _context;

        public VitalPatientApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<VitalPatient>>> GetVitalPatients()
        {
            return await _context.VitalPatients
                .Include(vp => vp.Vitals)
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<VitalPatient>> GetVitalPatient(int id)
        {
            var vitalPatient = await _context.VitalPatients
                .Include(vp => vp.Vitals)
                .FirstOrDefaultAsync(vp => vp.VitalPatientId == id);

            if (vitalPatient == null)
            {
                return NotFound();
            }

            return vitalPatient;
        }

        [HttpPost]
        public async Task<ActionResult<VitalPatient>> PostVitalPatient(VitalPatient vitalPatient)
        {
            _context.VitalPatients.Add(vitalPatient);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetVitalPatient), new { id = vitalPatient.VitalPatientId }, vitalPatient);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutVitalPatient(int id, VitalPatient vitalPatient)
        {
            if (id != vitalPatient.VitalPatientId)
            {
                return BadRequest();
            }

            _context.Entry(vitalPatient).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VitalPatientExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVitalPatient(int id)
        {
            var vitalPatient = await _context.VitalPatients.FindAsync(id);
            if (vitalPatient == null)
            {
                return NotFound();
            }

            _context.VitalPatients.Remove(vitalPatient);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool VitalPatientExists(int id)
        {
            return _context.VitalPatients.Any(e => e.VitalPatientId == id);
        }
    }
}
