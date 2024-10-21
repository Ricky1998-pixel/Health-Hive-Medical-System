using Microsoft.AspNetCore.Mvc;
using Health_Hive_Project_2024.Data;
using Health_Hive_Project_2024.Models;
using Health_Hive_Project_2024.ViewModels;

namespace Health_Hive_Project_2024.Controllers
{
    public class AdminReportController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminReportController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var viewModel = new AdminReportViewModel
            {
                HospitalDetails = _context.DayHospitals.FirstOrDefault() ?? new DayHospital(),
                BedsCount = _context.BedRecords.Count(),
                MedicationsCount = _context.MedicationRecords.Count(),
                UsersCount = _context.MedicalProfessionalRecords.Count(),
                TheatresCount = _context.OperatingTheatreRecords.Count(),
                ActiveIngredientsCount = _context.ActiveIngredientRecords.Count(),
                Medications = _context.MedicationRecords.ToList(),
                Users = _context.MedicalProfessionalRecords.ToList(),
                Theatres = _context.OperatingTheatreRecords.ToList(),
                Beds = _context.BedRecords.ToList()
            };

            return View(viewModel);
        }
    }
}



