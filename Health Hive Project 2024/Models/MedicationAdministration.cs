using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Health_Hive_Project_2024.Models
{
    public class MedicationAdministration
    {
        [Key]
        public int AdministrationID { get; set; }


        [ForeignKey("Medication")]
        [Display(Name = "Medication ID")]
        public int MedicationID { get; set; }
        public virtual MedicationRecords Medication { get; set; }

        [ForeignKey("Nurse")]
        [Display(Name = "Nurse")]
        public string? Id { get; set; }
        public virtual MedicalProfessionalRecords Nurse { get; set; }

        [ForeignKey("Patient")]
        public int PatientID { get; set; }
        public virtual Patient Patient { get; set; }
    }
}
