using Health_Hive_Project_2024.Data;
using Health_Hive_Project_2024.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Health_Hive_Project_2024.Controllers
{
    public class NotificationController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmailService _emailService;

        private readonly UserManager<MedicalProfessionalRecords> _userManager;

        public NotificationController(ApplicationDbContext context, IEmailService emailService, UserManager<MedicalProfessionalRecords> userManager)
        {
            _context = context;
            _emailService = emailService; // Injected Email Service
            _userManager = userManager; // Injected UserManager
        }
        public IActionResult Index()
        {
            var history = _context.GetNortifications.ToList(); // Ensure to load the notifications into a list
            return View(history);
        }
        [HttpPost]
        public async Task<IActionResult> DeleteSelected(List<int> selectedItems)
        {
            if (selectedItems != null && selectedItems.Any())
            {
                // Find and remove the selected items from the database
                var itemsToDelete = _context.GetNortifications.Where(h => selectedItems.Contains(h.NotifyId));
                _context.GetNortifications.RemoveRange(itemsToDelete);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index"); // Redirect back to the list
        }
    }
}
