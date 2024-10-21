using Health_Hive_Project_2024.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Health_Hive_Project_2024.Controllers
{
    public class SearchEngineController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SearchEngineController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // Retrieve patients who exist in SurgeryBooking only
            var patientsWithSurgery = await _context.Patients
                .Where(p => _context.PatientAdmissions.Any(sb => sb.PatientID == p.PatientID))
                .ToListAsync();

            return View(patientsWithSurgery);
        }


        [HttpGet]
        public IActionResult GetInfor(string searchText)
        {
            var filteredPatients = _context.Patients
                 .Where(p => _context.PatientAdmissions.Any(sb => sb.PatientID == p.PatientID))
                .Where(p => p.PatientIDNumber.Contains(searchText))
                .ToList();

            // Return a partial view to dynamically replace the table rows
            return PartialView("_PatientRows", filteredPatients);
        }



    }

    public static class HttpRequestExtensions
    {
        public static bool IsAjaxRequest(this HttpRequest request)
        {
            return request.Headers["X-Requested-With"] == "XMLHttpRequest";
        }
    }

}
