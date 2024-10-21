using Health_Hive_Project_2024.Data.ViewModels;
using Health_Hive_Project_2024.Data;
using Microsoft.AspNetCore.Mvc;

namespace Health_Hive_Project_2024.Controllers
{
    public class ViewAdmitedPatientsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ViewAdmitedPatientsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            // Fetch data for the AdmitPat model (replace with actual data retrieval)
            var patients = _context.Patients.ToList();  // Fetch patients
            var patientVitals = _context.PatientVitals.ToList();  // Fetch vitals
            /*var history = _context.History.FirstOrDefault();*/ // Fetch patient history

            // Construct the AdmitPat model
            var admitPatViewModel = patients.Select(patient => new AdmitPat
            {
                //Patient = patients.FirstOrDefault(),
                PatientVitals = patientVitals,
                //History = history
            }).ToList();

            return View(admitPatViewModel);  // Return the view
        }
    }

}
