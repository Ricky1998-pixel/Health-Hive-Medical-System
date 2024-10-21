// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using Health_Hive_Project_2024.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Health_Hive_Project_2024.Models;


namespace Health_Hive_Project_2024.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<MedicalProfessionalRecords> _signInManager;
        private readonly UserManager<MedicalProfessionalRecords> _userManager;
        private readonly IUserStore<MedicalProfessionalRecords> _userStore;
        private readonly IUserEmailStore<MedicalProfessionalRecords> _emailStore;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _dbContext; // Use ApplicationDbContext
        public RegisterModel(
            UserManager<MedicalProfessionalRecords> userManager,
            IUserStore<MedicalProfessionalRecords> userStore,
            SignInManager<MedicalProfessionalRecords> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext dbContext) // Use ApplicationDbContext here

        {
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _roleManager = roleManager;
            _dbContext = dbContext;
        }

        [BindProperty]
        public InputModel Input { get; set; }
        public IList<SelectListItem> Role { get; set; }
        public IList<SelectListItem> TitleOptions { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "Title is required")]
            [Display(Name = "Title")]
            public string Title { get; set; }
            public List<SelectListItem> TitleOptions { get; set; }

            [Required(ErrorMessage = "Name is required")]
            [Display(Name = "Name")]
            public string Name { get; set; }

            [Required(ErrorMessage = "Name is required")]
            [Display(Name = "Name")]
            [StringLength(6)]
            public string HealthCouncilRegistrationNumber { get; set; }

            [Required(ErrorMessage = "Surname is required")]
            [Display(Name = "Surname")]
            public string Surname { get; set; }

            [Required(ErrorMessage = "Address is required")]
            [Display(Name = "Address")]
            public string Address { get; set; }

            [Required(ErrorMessage = "Professional Contact Number is required")]
            [Display(Name = "Contact Number")]
            public string ContactNumber { get; set; }

            //[Required(ErrorMessage = "Postal Code is required")]
            [Display(Name = "Postal Code")]
            public string PostalCode { get; set; }

            //[Required(ErrorMessage = "Suburb is required")]
            [Display(Name = "Suburb")]
            public string Suburb { get; set; }

            //[Required(ErrorMessage = "City is required")]
            [Display(Name = "City")]
            public string City { get; set; }

            //[Required(ErrorMessage = "Province is required")]
            [Display(Name = "Province")]
            public string Province { get; set; }

            [Required(ErrorMessage = "Email is required")]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

            // Add a property for Role selection during registration
            [Required(ErrorMessage = "Role is required")]
            [Display(Name = "Role")]
            public string Role { get; set; } = "Admin";



        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            // Initialize TitleOptions before populating it
            Input = new InputModel
            {
                TitleOptions = new List<SelectListItem>()
            };

            // Populate title dropdown options
            Input.TitleOptions.AddRange(new[]
            {
        new SelectListItem { Value = "Mr.", Text = "Mr." },
        new SelectListItem { Value = "Ms.", Text = "Ms." },
        new SelectListItem { Value = "Mrs.", Text = "Mrs." }
    });

            // Populate the Role property with available roles from RoleManager
            Role = _roleManager.Roles.Select(r => new SelectListItem { Text = r.Name, Value = r.Name }).ToList();

        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            if (ModelState.IsValid)
            {
                var user = new MedicalProfessionalRecords {
                    UserName = Input.Email,
                    Email = Input.Email,
                    Name = Input.Name,
                    Surname = Input.Surname,
                    HealthCouncilRegistrationNumber= Input.HealthCouncilRegistrationNumber,
                    ContactNumber=Input.ContactNumber,
                    EmailAddress = Input.Email
                };
                var result = await _userManager.CreateAsync(user, Input.Password);                
                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    // Assign role to the user based on selection during registration
                    if (!await _roleManager.RoleExistsAsync(Input.Role))
                    {
                        await _roleManager.CreateAsync(new IdentityRole(Input.Role));
                    }
                    await _userManager.AddToRoleAsync(user, Input.Role);

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = user.Id, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    // Set a success message in TempData
                    TempData["SuccessMessage"] = "Welcome Admin: You have been registered to the Health Hive System";

                    // Redirect to the login page where the modal will be shown
                    return RedirectToPage("/Account/Login", new { area = "Identity" });
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }


        private IUserEmailStore<MedicalProfessionalRecords> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<MedicalProfessionalRecords>)_userStore;
        }
    }
}


