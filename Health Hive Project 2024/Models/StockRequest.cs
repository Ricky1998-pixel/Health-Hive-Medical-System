using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Health_Hive_Project_2024.Models
{
    public class StockRequest
    {
        [Key]
        public int RequestID { get; set; }


        [ForeignKey("Pharmacist")]
        public string? PharmacistID { get; set; }
        public virtual MedicalProfessionalRecords Pharmacist { get; set; }

        public string Quantity { get; set; }
        public DateTime RequestDate { get; set; }
        public string Status { get; set; }


        [ForeignKey("Medication")]
        [Display(Name = "Medication ID")]
        public int MedicationID { get; set; }
        public virtual MedicationRecords Medication { get; set; }

        public DateTime? FulfillmentDate { get; set; }
    }
}
