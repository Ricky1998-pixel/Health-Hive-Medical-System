using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Health_Hive_Project_2024.Models
{
    public class ActiveIngredientRecords
    { 
        [Key]
        public int IngredientID { get; set; }

        [Required(ErrorMessage = "Ingredient Name is required")]
        [Display(Name = "Active Ingredient Name")]
        public string IngredientName { get; set; }

    }
}
