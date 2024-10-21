using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Health_Hive_Project_2024.Models
{
    public class PatientMedication
    {
        [Key]
        public int PatientMedicationID { get; set; }

        [ForeignKey("Patient")]
        [Display(Name = "Patient ID")]
        public int PatientID { get; set; }
        public virtual Patient Patient { get; set; }

        [ForeignKey("Medication")]
        [Display(Name = "Medication ID")]
        public int MedicationID { get; set; }
        public virtual MedicationRecords Medication { get; set; }
    }
}
