using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Health_Hive_Project_2024.Models
{
    public class MedicalHistoryMedication
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("MedicalHistory")]
        public int MedicalHistoryID { get; set; }
        public MedicalHistory MedicalHistory { get; set; }

        [ForeignKey("MedicationRecords")]
        public int MedicationID { get; set; }
        public MedicationRecords MedicationRecords { get; set; }
    }
}