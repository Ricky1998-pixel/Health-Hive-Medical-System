using System.ComponentModel.DataAnnotations.Schema;

namespace Health_Hive_Project_2024.Models
{
    public class DispensedPrescription
    {
        public int DispensedPrescriptionID { get; set; } // Primary key
        public int PrescriptionID { get; set; }
        public string SurgeonID { get; set; }
        [ForeignKey("SurgeonID")]
        public virtual MedicalProfessionalRecords Surgeon { get; set; } // Surgeon Navigation Property
        public int PatientID { get; set; }
        [ForeignKey("PatientID")]
        public virtual Patient Patient { get; set; } // Patient Navigation Property
        public DateTime Date { get; set; }
        public string PrescriptionStatus { get; set; }
        public string? PharmacistID { get; set; }
        [ForeignKey("PharmacistID")]
        public virtual MedicalProfessionalRecords Pharmacist { get; set; } // Pharmacist Navigation Property
        public int MedicationID { get; set; }
        [ForeignKey("MedicationID")]
        public virtual MedicationRecords Medication { get; set; } // Medication Navigation Property
        public int Quantity { get; set; }
        public string Instructions { get; set; }
    }

}
