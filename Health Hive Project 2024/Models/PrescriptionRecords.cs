using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Health_Hive_Project_2024.Models
{
    public class PrescriptionRecords
    {

        [Key]
        public int PrescriptionID { get; set; }

        [ForeignKey("Surgeon")]
        public string? SurgeonID { get; set; }
        public virtual MedicalProfessionalRecords Surgeon { get; set; }

        [ForeignKey(nameof(Nurse))]
        public string? NurseID { get; set; }
        public virtual MedicalProfessionalRecords? Nurse { get; set; }


        [ForeignKey("Patient")]
        public int PatientID { get; set; }
        public virtual Patient? Patient { get; set; }

        public DateTime Date { get; set; } = DateTime.Now;
        public string PrescriptionStatus { get; set; }

        [ForeignKey("Pharmacist")]
        public string? PharmacistID { get; set; }
        public virtual MedicalProfessionalRecords? Pharmacist { get; set; }
        public virtual ICollection<PrescriptionMedications> PrescriptionMedications { get; set; }

    }
}
