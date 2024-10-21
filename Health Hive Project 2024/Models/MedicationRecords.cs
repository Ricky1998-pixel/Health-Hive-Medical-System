using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Health_Hive_Project_2024.Models
{
    public class MedicationRecords
    {
        [Key]
        public int MedicationID { get; set; }

        //[Required(ErrorMessage = "Medication Name is required")]
        [Display(Name = "Medication Name")]
        public string MedicationName { get; set; }

        [ForeignKey("DosageForm")]
        [Display(Name = "Dosage Form ID")]
        public int DosageFormID { get; set; }
        public virtual DosageForm DosageForm { get; set; }

        //[Required(ErrorMessage = "Schedule is required")]
        [Display(Name = "Schedule")]
        public string Schedule { get; set; }

        //[Required(ErrorMessage = "Quantity of medication on hand is required")]
        [Display(Name = "Quantity Of Medication")]
        public string QuantityOnHand { get; set; }

        //[Required(ErrorMessage = "Re-Order Level is required")]
        [Display(Name = "Re-Order Level")]
        public string ReOrderLevel { get; set; }

        // Navigation property
        public ICollection<MedicationActiveIngredient> MedicationActiveIngredients { get; set; }
    }
}


