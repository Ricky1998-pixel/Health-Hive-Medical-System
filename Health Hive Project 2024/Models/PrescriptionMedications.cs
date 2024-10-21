
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Health_Hive_Project_2024.Models
{
    public class PrescriptionMedications
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("PrescriptionRecords")]
        public int PrescriptionID { get; set; }
        public PrescriptionRecords PrescriptionRecords { get; set; }

        [ForeignKey("MedicationRecords")]
        public int MedicationID { get; set; }
        public MedicationRecords MedicationRecords { get; set; }

        public int Quantity { get; set; }

        public string Instructions { get; set; }
        public int QuantityAdministered { get; set; } = 0;
        public DateTime? AdministeredDate { get; set; } = DateTime.Now;
        
    }
}

