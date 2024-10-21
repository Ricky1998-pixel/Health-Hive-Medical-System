using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Net.Mail;
using System.Linq;
using Microsoft.AspNetCore.Identity;

namespace Health_Hive_Project_2024.Models
{
    public enum SpecializationType
    {
        Admin,
        Nurse,
        Pharmacist,
        Surgeon,
        Anesthesiologist
        
    }

    public class MedicalProfessionalRecords : IdentityUser
    {//comment in medicalProffesionalID
        


        [Required(ErrorMessage = "Professional Name is required")]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Professional Surname is required")]
        [Display(Name = "Surname")]
        public string Surname { get; set; }

        [Required(ErrorMessage = "Professional Contact Number is required")]
        [Display(Name = "Contact Number")]
        [StringLength(10)]
        public string ContactNumber { get; set; }

        [Required(ErrorMessage = "Professional Email Address is required")]
        [EmailAddress]
        [Display(Name = "Email Address")]
        public string? EmailAddress { get; set; }

        [Required(ErrorMessage = "Professional Specialization is required")]
        [Display(Name = "Specialization")]
        public SpecializationType Specialization { get; set; }

        
        [StringLength(6)]
        public string? HealthCouncilRegistrationNumber { get; set; }

    }



}

