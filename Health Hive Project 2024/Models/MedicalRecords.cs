using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Health_Hive_Project_2024.Models
{
    public class MedicalRecords
    {
        [Key]
        public int RecordsID { get; set; }

        //[ForeignKey("Patient")]
        //[Display(Name = "Patient")]
        //public int PatientID { get; set; }
        //public virtual Patient Patient { get; set; }

        [ForeignKey("Allergies")]
        [Display(Name = "Allergies")]
        public int AllergyID { get; set; }
        public virtual Allergies Allergies { get; set; }

        //[ForeignKey("Medication")]
        //[Display(Name = "Medication ID")]
        //public int MedicationID { get; set; }
        //public virtual MedicationRecords Medication { get; set; }

        public string CurrentMedication { get; set; }
    }
}
