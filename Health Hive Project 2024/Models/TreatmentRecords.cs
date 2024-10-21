using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Health_Hive_Project_2024.Models
{
    public class TreatmentRecords
    {
        [Key]
        public int TreatmentID { get; set; }

        [Required(ErrorMessage = "Treatment code is required")]
        [Display(Name = "Treatment Code")]
        public string TreatmentCode { get; set; }

        [Required(ErrorMessage = "Description is required")]
        [Display(Name = "Description")]
        public string Description { get; set; }
        public virtual ICollection<SurgeryBookingTreatment>? SurgeryBookingTreatments { get; set; }
    }
}

