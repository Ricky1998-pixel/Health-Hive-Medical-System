using Health_Hive_Project_2024.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Health_Hive_Project_2024.Controllers
{
    public class ChatController : Controller
    {
        private readonly UserManager<MedicalProfessionalRecords> _userManager;

        public ChatController(UserManager<MedicalProfessionalRecords> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            // Get list of users from AspNetUsers table
            var users = _userManager.Users
                .Select(u => new { u.Id, u.UserName })
                .ToList();

            ViewBag.Users = users;
            return View();
        }

    }
}


