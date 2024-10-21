using System.ComponentModel.DataAnnotations.Schema;

namespace Health_Hive_Project_2024.Models
{
    public class RejectedPrescription
    {
        //public int RejectedPrescriptionID { get; set; } // Primary Key
        //public int PrescriptionID { get; set; } // Reference to original PrescriptionID
        //public string SurgeonID { get; set; }
        //public int PatientID { get; set; }
        //public DateTime Date { get; set; }
        //public string PrescriptionStatus { get; set; } = "Pending"; // Default status
        //public string? PharmacistID { get; set; }
        //public int MedicationID { get; set; }
        //public int Quantity { get; set; }
        //public string Instructions { get; set; }
        //public string RejectionReason { get; set; } // Reason for rejection
        //public DateTime RejectedOn { get; set; } = DateTime.Now; // Date of rejection


        public int RejectedPrescriptionID { get; set; } // Primary Key
        public int PrescriptionID { get; set; } // Reference to original PrescriptionID

        public string SurgeonID { get; set; }
        [ForeignKey("SurgeonID")]
        public virtual MedicalProfessionalRecords Surgeon { get; set; } // Surgeon Navigation Property

        public int PatientID { get; set; }
        [ForeignKey("PatientID")]
        public virtual Patient Patient { get; set; }
        public DateTime Date { get; set; }
        public string PrescriptionStatus { get; set; } = "Pending"; // Default status

        public string? PharmacistID { get; set; }
        [ForeignKey("PharmacistID")]
        public virtual MedicalProfessionalRecords Pharmacist { get; set; } // Pharmacist Navigation Property

        public int MedicationID { get; set; }
        [ForeignKey("MedicationID")]
        public virtual MedicationRecords Medication { get; set; } // Medication Navigation Property

        public int Quantity { get; set; }
        public string Instructions { get; set; }
        public string RejectionReason { get; set; } // Reason for rejection
        public DateTime RejectedOn { get; set; } = DateTime.Now; // Date of rejection
    }

}
