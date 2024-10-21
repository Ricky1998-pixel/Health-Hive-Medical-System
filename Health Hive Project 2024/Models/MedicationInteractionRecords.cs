using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Health_Hive_Project_2024.Models
{
    public class MedicationInteractionRecords
    {
        [Key]
        public int InteractionID { get; set; }

        [ForeignKey("Medication")]
        [Display(Name = "Medication ID")]
        public int MedicationID { get; set; }
        public virtual MedicationRecords Medication { get; set; }


        [Required(ErrorMessage = "Interaction Type is required")]
        [Display(Name = "Interaction Type")]
        public string InteractionType { get; set; }

        [Required(ErrorMessage = "Severity")]
        [Display(Name = "Severity")]
        public string Severity { get; set; }

        [Required(ErrorMessage = "Description")]
        [Display(Name = "Description")]
        public string Description { get; set; }


        [ForeignKey("Admin")]
        [Display(Name = "Admin")]
        public string? AdminID { get; set; }
        public virtual MedicalProfessionalRecords Admin { get; set; }

    }
}