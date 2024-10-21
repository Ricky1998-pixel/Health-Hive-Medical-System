using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Health_Hive_Project_2024.Models
{
    public class MedicalHistory
    {
        [Key]
        public int MedicalHistoryID { get; set; }

        [ForeignKey("Patient")]
        [Display(Name = "Patient")]
        public int PatientID { get; set; }
        public virtual Patient Patient { get; set; }

        [Required]
        public DateTime DateRecorded { get; set; }

        public virtual ICollection<MedicalHistoryAllergy> MedicalHistoryAllergies { get; set; }

        public virtual ICollection<MedicalHistoryMedication> MedicalHistoryMedications { get; set; }

        public virtual ICollection<MedicalHistoryCondition> MedicalHistoryCondition { get; set; }

    }
}




