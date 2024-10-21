using Health_Hive_Project_2024.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Health_Hive_Project_2024.Data.ViewModels
{
    public class AdmitPat
    {
        public PatientVMM Patient { get; set; }
        public IEnumerable<PatientVitals> PatientVitals { get; set; }
        public History? History { get; set; }
        public int Ward { get; set; }
        public int Bed { get; set; }
        public int Height { get; set; }
        public int Weight { get; set; }
        public string BookingID { get; set; }


    }
    public class PatientVMM
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

    }

    public class ViewPatVitals
    {
        public Vitals Vitals { get; set; }
        public IEnumerable<PatientVitals> PatientVitals { get; set; }

    }
    public class Bp
    {
        public int s { get; set; }
        public int d { get; set; }
    }
    public class History
    {
        public List<Data>? Conditions { get; set; }
        public List<Data>? Allergies { get; set; }
        public List<Data>? Medicines { get; set; }
    }
    public class Data
    {
        public int Id { get; set; }
        public int? PatientId { get; set; }
        public string name { get; set; }
    }
    public class DashboardCount
    {
        public int Administer { get; set; }
        public int Booking { get; set; }
        public int Count { get; set; }
        public int Admitted { get; set; }
        public int Vitals { get; set; }
        public int Discharge { get; set; }
    }
    public class Addmini
    {
        public int Id { get; set; }
        public int Count { get; set; }
        public DateTime DateTime { get; set; }
    }
    public class ViewPres
    {
        public PrescriptionRecords Pres { get; set; }
        public IEnumerable<MedList> Items { get; set; }
    }
    public class MedList
    {
        public string Name { get; set; }
        public int qty { get; set; }
        public string Inst { get; set; }
        public int AdQty { get; set; }
        public int id { get; set; }
    }

}
