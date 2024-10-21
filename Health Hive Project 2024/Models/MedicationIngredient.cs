using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Humanizer;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Health_Hive_Project_2024.Models
{
    public class MedicationIngredient
    {
        [Key]
        public int MedicationIngredientID { get; set; }

        [ForeignKey("Medication")]
        [Display(Name = "Medication ID")]
        public int MedicationID { get; set; }
        public virtual MedicationRecords Medication { get; set; }

        [ForeignKey("Ingredient")]
        [Display(Name = "Ingredient ID")]
        public int IngredientID { get; set; }
        public virtual ActiveIngredientRecords Ingredient { get; set; }

        [Required(ErrorMessage = "Active Ingredient Strength is required")]
        [Display(Name = "Active Ingredient Strength")]
        public string ActiveIngredientStrength { get; set; }


    }
}
