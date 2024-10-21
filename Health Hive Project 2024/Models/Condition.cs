using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Health_Hive_Project_2024.Models
{
    public class Condition
    {
        [Key]
        public int ConditionID { get; set; }

        //[ForeignKey("PatientID")]
        //public int PatientID { get; set; }
        //public virtual Patient Patient { get; set; }


        public string? CODE { get; set; }
        public string? Diagnosis { get; set; }


        // Collection of medications that are contraindicated for this condition
        public ICollection<MedicationRecords> ContraindicatedMedications { get; set; }

    }
}