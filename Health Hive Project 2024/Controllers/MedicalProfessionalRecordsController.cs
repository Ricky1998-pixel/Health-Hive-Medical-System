using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Health_Hive_Project_2024.Data;
using Health_Hive_Project_2024.Models;
using Health_Hive_Project_2024; // Include the namespace for the email service
using Microsoft.AspNetCore.Identity;



namespace Health_Hive_Project_2024.Controllers

{
    public class MedicalProfessionalRecordsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmailService _emailService;

        private readonly UserManager<MedicalProfessionalRecords> _userManager;

        public MedicalProfessionalRecordsController(ApplicationDbContext context, IEmailService emailService, UserManager<MedicalProfessionalRecords> userManager)
        {
            _context = context;
            _emailService = emailService; // Injected Email Service
            _userManager = userManager; // Injected UserManager
        }

        // GET: MedicalProfessionalRecords
        public async Task<IActionResult> Index()
        {
            return View(await _context.MedicalProfessionalRecords.ToListAsync());
        }

        // GET: MedicalProfessionalRecords/Details/5
        public async Task<IActionResult> Details(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }
           
            var medicalProfessionalRecords = await _context.MedicalProfessionalRecords
                .FirstOrDefaultAsync(m => m.Id == id);//from medprof to Id
            if (medicalProfessionalRecords == null)
            {
                return NotFound();
            }

            return View(medicalProfessionalRecords);
        }

        // GET: MedicalProfessionalRecords/Create
        public IActionResult Create()
        {
            return View();
        }

        //// POST: MedicalProfessionalRecords/Create
        // POST: MedicalProfessionalRecords/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MedicalProfessionalID,Name,Surname,ContactNumber,EmailAddress,Specialization,HealthCouncilRegistrationNumber")] MedicalProfessionalRecords medicalProfessionalRecords)
        {
            if (ModelState.IsValid)
            {
                using (var transaction = await _context.Database.BeginTransactionAsync())
                {
                    try
                    {
                        // Add the medical professional record
                        //context.Add(medicalProfessionalRecords);
                        //await _context.SaveChangesAsync();

                        // Generate auto-generated password
                        string password = GenerateRandomPassword();

                        //Create the ApplicationUser
                        //var user = new IdentityUser
                        var user = new MedicalProfessionalRecords
                        {
                            UserName = medicalProfessionalRecords.EmailAddress,
                            EmailAddress = medicalProfessionalRecords.EmailAddress,
                            Email = medicalProfessionalRecords.EmailAddress,
                            Name = medicalProfessionalRecords.Name,
                            Surname = medicalProfessionalRecords.Surname,
                            HealthCouncilRegistrationNumber = medicalProfessionalRecords.HealthCouncilRegistrationNumber,
                            ContactNumber = medicalProfessionalRecords.ContactNumber,
                            Specialization = medicalProfessionalRecords.Specialization
                        };

                        // Create the user in ASP.NET Identity
                        var result = await _userManager.CreateAsync(user, password);

                        if (result.Succeeded)
                        {
                            // Add user to roles based on specialization
                            await AssignRoleBasedOnSpecialization(user,medicalProfessionalRecords.Specialization.ToString());
                            await _userManager.AddToRoleAsync(user, medicalProfessionalRecords.Specialization.ToString());
                            // Send email to the professional
                            await _emailService.SendEmailAsync(medicalProfessionalRecords.EmailAddress, medicalProfessionalRecords.Name, medicalProfessionalRecords.Surname, medicalProfessionalRecords.EmailAddress, password);

                            await transaction.CommitAsync();

                            //return RedirectToAction(nameof(Index));
                            return Json(new { success = true });
                        }
                        else
                        {
                            foreach (var error in result.Errors)
                            {
                                ModelState.AddModelError(string.Empty, error.Description);
                            }
                        }

                        // If user creation failed, roll back the transaction
                        await transaction.RollbackAsync();
                    }
                    catch
                    {
                        // Roll back the transaction if any error occurs
                        await transaction.RollbackAsync();
                        ModelState.AddModelError(string.Empty, "An error occurred while creating the medical professional record.");
                    }
                }
            }

            // If we got to this point, something failed, redisplay form
            //return View(medicalProfessionalRecords);
            return Json(new { success = false, errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
        }


        // GET: MedicalProfessionalRecords/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medicalProfessionalRecords = await _context.MedicalProfessionalRecords.FindAsync(id);
            if (medicalProfessionalRecords == null)
            {
                return NotFound();
            }
            return View(medicalProfessionalRecords);
        }

        // POST: MedicalProfessionalRecords/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("MedicalProfessionalID,Name,Surname,ContactNumber,EmailAddress,Specialization,HealthCouncilRegistrationNumber")] MedicalProfessionalRecords medicalProfessionalRecords)
        {
            if (id != medicalProfessionalRecords.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(medicalProfessionalRecords);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MedicalProfessionalRecordsExists(medicalProfessionalRecords.Id))
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
            return View(medicalProfessionalRecords);
        }

        //// GET: MedicalProfessionalRecords/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var medicalProfessionalRecords = await _context.MedicalProfessionalRecords
        //        .FirstOrDefaultAsync(m => m.MedicalProfessionalID == id);
        //    if (medicalProfessionalRecords == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(medicalProfessionalRecords);
        //}

        //// POST: MedicalProfessionalRecords/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var medicalProfessionalRecords = await _context.MedicalProfessionalRecords.FindAsync(id);
        //    if (medicalProfessionalRecords != null)
        //    {
        //        _context.MedicalProfessionalRecords.Remove(medicalProfessionalRecords);
        //    }

        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}


        // GET: MedicalProfessionalRecords/Delete/5
        public async Task<IActionResult> Delete(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medicalProfessionalRecords = await _context.MedicalProfessionalRecords
                .FirstOrDefaultAsync(m => m.Id == id);
            if (medicalProfessionalRecords == null)
            {
                return NotFound();
            }

            return View(medicalProfessionalRecords);
        }

        // POST: MedicalProfessionalRecords/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var medicalProfessionalRecords = await _context.MedicalProfessionalRecords
                .FirstOrDefaultAsync(m => m.Id == id);

            if (medicalProfessionalRecords != null)
            {
                // Find and delete the user with matching email
                var user = await _context.Users
                    .FirstOrDefaultAsync(u => u.UserName == medicalProfessionalRecords.EmailAddress);

                if (user != null)
                {
                    _context.Users.Remove(user);
                }

                // Delete the medical professional record
                _context.MedicalProfessionalRecords.Remove(medicalProfessionalRecords);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool MedicalProfessionalRecordsExists( string id)
        {
            return _context.MedicalProfessionalRecords.Any(e => e.Id == id);
        }
        
        //New Code for Password= Password123!
        private string GenerateRandomPassword()
        {
            // Define a constant password to always return
            const string password = "Password123!";

            // Return the predefined password
            return password;
        }

        // Helper method to assign role based on specialization
        private async Task AssignRoleBasedOnSpecialization(MedicalProfessionalRecords user, string specialization)
        {
            switch (specialization.ToLower())
            {
                case "pharmacist":
                    await _userManager.AddToRoleAsync(user, "Pharmacist");
                    break;
                case "surgeon":
                    await _userManager.AddToRoleAsync(user, "Surgeon");
                    break;
                case "nurse":
                    await _userManager.AddToRoleAsync(user, "Nurse");
                    break;
                case "anesthesiologist":
                    await _userManager.AddToRoleAsync(user, "Anesthesiologist");
                    break;

                case "admin":
                    await _userManager.AddToRoleAsync(user, "Admin");
                    break;

                default:
                    
                    break;
            }
        }

    }
}

