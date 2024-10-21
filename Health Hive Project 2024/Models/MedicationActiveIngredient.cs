using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Health_Hive_Project_2024.Models
{
    public class MedicationActiveIngredient
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("MedicationRecords")]
        public int MedicationID { get; set; }
        public MedicationRecords MedicationRecords { get; set; }

        [ForeignKey("ActiveIngredientRecords")]
        public int IngredientID { get; set; }
        public ActiveIngredientRecords ActiveIngredientRecords { get; set; }
    }
}

