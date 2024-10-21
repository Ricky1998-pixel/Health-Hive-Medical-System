using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Health_Hive_Project_2024.Models
{
    public class PrescriptionItem
    {
        [Key]
        public int PrescriptionItemID { get; set; }

        [ForeignKey("Prescription")]
        public int PrescriptionID { get; set; }
        public virtual PrescriptionRecords Prescription { get; set; }

        [ForeignKey("Medication")]
        public int MedicationID { get; set; }
        public virtual MedicationRecords Medication { get; set; }

        [Required(ErrorMessage = "Quantity of medication is required")]
        [Display(Name = "Quantity")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "Medicine Instructions are required")]
        [Display(Name = "Instructions")]
        public string Instructions { get; set; }
        public int QuantityAdministered { get; set; } = 0;
        public DateTime AdministeredDate { get; set; }

    }
}
