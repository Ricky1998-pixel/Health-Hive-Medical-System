using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Health_Hive_Project_2024.Models
{
    public class MedicationAlert
    {
        [Key]
        public int MedicationAlertID { get; set; }


        [ForeignKey("Ingredient")]
        [Display(Name = "Ingredient ID")]
        public int IngredientID { get; set; }
        public virtual ActiveIngredientRecords Ingredient { get; set; }
    }
}
