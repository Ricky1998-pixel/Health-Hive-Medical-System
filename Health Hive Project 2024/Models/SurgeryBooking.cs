using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Health_Hive_Project_2024.Models
{
    public class SurgeryBooking
    {
        [Key]
        public int SurgeryID { get; set; }

        [ForeignKey("Surgeon")]
        [Display(Name = "Surgeon")]
        public string? SurgeonID { get; set; }
        public virtual MedicalProfessionalRecords? Surgeon { get; set; }

        [ForeignKey("Patient")]
        [Display(Name = "Patient")]
        public int? PatientID { get; set; }
        public virtual Patient? Patient { get; set; }
        [Display(Name = "Surgery Date")]
        public DateTime SurgeryDate { get; set; }

        [Required(ErrorMessage = "Session is required")]
        [Display(Name = "Session")]
        public string? Session { get; set; }

        [ForeignKey("Anaesthesiologist")]
        [Display(Name = "Anaesthesiologist")]
        public string? AnaesthesiologistID { get; set; }
        public virtual MedicalProfessionalRecords? Anaesthesiologist { get; set; }

        [ForeignKey("Theatre")]
        [Display(Name = "Theatre")]
        public int? TheatreID { get; set; }
        public virtual OperatingTheatreRecords? Theatre { get; set; }
        public bool IsActive { get; set; } = true;

        public virtual ICollection<SurgeryBookingTreatment> SurgeryBookingTreatments { get; set; }


    }
}
