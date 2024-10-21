using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Health_Hive_Project_2024.Models
{
    public class ContraIndicationsRecords
    {
        [Key]
        public int ContraIndicationID { get; set; }

        [ForeignKey("Condition")]
        [Display(Name = "Condition")]
        public int ConditionID { get; set; }
        public virtual Condition Condition { get; set; }

        [ForeignKey("Ingredient")]
        [Display(Name = "Ingredient ID")]
        public int IngredientID { get; set; }
        public virtual ActiveIngredientRecords Ingredient { get; set; }

        [Required(ErrorMessage = "Alert Message is required")]
        [Display(Name = "Alert Message Description")]
        public string AlertMessage { get; set; }

        public string AlertType { get; set; }
    }
}
