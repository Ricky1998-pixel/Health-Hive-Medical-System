using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Health_Hive_Project_2024.Models
{
    public class Patient
    {
        [Key]
        public int PatientID { get; set; }

        [Required(ErrorMessage = "Patient ID number is required")]
        [Display(Name = "ID Number")]
        [StringLength(13, MinimumLength = 13, ErrorMessage = "Patient ID number must be exactly 13 digits")]
        [RegularExpression(@"^\d{13}$", ErrorMessage = "Patient ID number must contain only digits")]
        public string PatientIDNumber { get; set; }

        [Required(ErrorMessage = "Patient Name is required")]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Patient Surname is required")]
        [Display(Name = "Surname")]
        public string Surname { get; set; }

        [Required(ErrorMessage = "Patient Address is required")]
        [Display(Name = "Address")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Patient Cell number is required")]
        [Display(Name = "Contact Number")]
        public string ContactNumber { get; set; }

        [Required(ErrorMessage = "Patient Email Address is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [Display(Name = "Email Address")]
        public string EmailAddress { get; set; }

        [Required(ErrorMessage = "Patient Date Of Birth is required")]
        [Display(Name = "Date of Birth")]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Patient Gender is required")]
        [Display(Name = "Gender")]
        public string Gender { get; set; }

        [NotMapped]
        public virtual IEnumerable<Vitals>? Vitals { get; set; } // worry not it wont affect the DB

        // Relationship to Medical History
        public ICollection<MedicalHistory> MedicalHistories { get; set; }
    }
}
