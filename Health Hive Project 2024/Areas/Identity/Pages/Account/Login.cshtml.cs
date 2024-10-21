// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Health_Hive_Project_2024.Models;

namespace Health_Hive_Project_2024.Areas.Identity.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly SignInManager<MedicalProfessionalRecords> _signInManager;
        private readonly IHttpContextAccessor http;
        private readonly ILogger<LoginModel> _logger;
        private readonly UserManager<MedicalProfessionalRecords> _userManager; // Add UserManager<MedicalProfessionalRecords>

        public LoginModel(SignInManager<MedicalProfessionalRecords> signInManager, UserManager<MedicalProfessionalRecords> userManager, ILogger<LoginModel> logger, IHttpContextAccessor http)
        {
            _signInManager = signInManager;
            _userManager = userManager; // Initialize UserManager<MedicalProfessionalRecords>
            _logger = logger;
            this.http = http;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string ReturnUrl { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string ErrorMessage { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl ??= Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User logged in.");
                    // Check if the logged-in user is in the Admin role
                    var user = await _userManager.FindByEmailAsync(Input.Email);
                    try
                    {
                        HttpContext.Session.SetString("userid", user.Id);
                        HttpContext.Session.SetString("user_name", user.Name);
                        HttpContext.Session.SetString("user_full_name", $"{user.Name} {user.Surname}");
                        HttpContext.Session.SetString("user_surname", user.Surname);
                    }
                    catch
                    {
                        ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                        return Page();
                    }

                    var isAdmin = await _userManager.IsInRoleAsync(user, "Admin");
                    if (isAdmin)
                    {
                        // Redirect to the admin dashboard if the user is an admin
                        return RedirectToAction("AdminDashboard", "Home");
                    }
                    // Check other roles and redirect accordingly
                    var isNurse = await _userManager.IsInRoleAsync(user, "Nurse");
                    if (isNurse)
                    {

                        return RedirectToAction("NurseDashboard", "Home");
                    }

                    var isSurgeon = await _userManager.IsInRoleAsync(user, "Surgeon");
                    if (isSurgeon)
                    {
                        return RedirectToAction("SurgeonDashboard", "Home");
                    }

                    var isPharmacist = await _userManager.IsInRoleAsync(user, "Pharmacist");
                    if (isPharmacist)
                    {
                        return RedirectToAction("PharmacistDashboard", "Home");
                    }

                    var isAnaesthesiologist = await _userManager.IsInRoleAsync(user, "Anesthesiologist");
                    if (isAnaesthesiologist)
                    {
                        return RedirectToAction("AnaesthesiologistDashboard", "Home");
                    }



                    return LocalRedirect(returnUrl);
                }
                if (result.RequiresTwoFactor)
                {
                    return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
                }
                if (result.IsLockedOut)
                {
                    _logger.LogWarning("User account locked out.");
                    return RedirectToPage("./Lockout");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return Page();
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
