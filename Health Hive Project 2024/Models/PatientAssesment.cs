using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Health_Hive_Project_2024.Models
{
    public class PatientAssesment
    {
        [Key]
        public int PatientAssesmentID { get; set; }

        [ForeignKey("Anaesthesiologist")]
        [Display(Name = "Anaesthesiologist")]
        public string Id { get; set; }
        public virtual MedicalProfessionalRecords Anaesthesiologist { get; set; }

        [ForeignKey("Patient")]
        [Display(Name = "Patient ID")]
        public int PatientID { get; set; }
        public virtual Patient Patient { get; set; }

        [ForeignKey("MedicalRecord")]  // Assuming MedicalRecord is the correct navigation property name
        [Display(Name = "Medical Record")]
        public int RecordsID { get; set; }
        public virtual MedicalRecords MedicalRecord { get; set; }

        public DateTime? AssasmentDate { get; set; }
    }
}

