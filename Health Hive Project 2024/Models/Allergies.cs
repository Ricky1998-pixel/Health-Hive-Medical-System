using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Health_Hive_Project_2024.Models
{
    public class Allergies
    {
        [Key]
        public int AllergyID { get; set; }


        [ForeignKey("Patient")]
        [Display(Name = "Patient ID")]
        public int PatientID { get; set; }
        public virtual Patient Patient { get; set; }

        [ForeignKey("Ingredient")]
        [Display(Name = "Ingredient ID")]
        public int IngredientID { get; set; }
        public virtual ActiveIngredientRecords Ingredient { get; set; }
    }
}
