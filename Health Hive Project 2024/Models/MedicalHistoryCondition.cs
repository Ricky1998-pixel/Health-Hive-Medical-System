using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Health_Hive_Project_2024.Models
{
    public class MedicalHistoryCondition
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("MedicalHistory")]
        public int MedicalHistoryID { get; set; }
        public MedicalHistory MedicalHistory { get; set; }

        [ForeignKey("Condition")]
        public int ConditionID { get; set; }
        public Condition Condition { get; set; }
    }
}