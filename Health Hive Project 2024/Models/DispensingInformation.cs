using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Health_Hive_Project_2024.Models
{
    public class DispensingInformation
    {
        [Key]
        public int DispensingID { get; set; }

        [ForeignKey("Prescription")]
        public int PrescriptionID { get; set; }
        public virtual PrescriptionRecords Prescription { get; set; }

        [Required(ErrorMessage = "Pharmacist name is required")]
        [Display(Name = "Name of Pharmacist")]
        public string DispensingPharmacist { get; set; } // Name of the dispensing pharmacist
        public DateTime DispensingDateTime { get; set; } // Date and time of dispensing
    }
}
