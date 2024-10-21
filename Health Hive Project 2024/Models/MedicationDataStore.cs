using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Health_Hive_Project_2024.Models
{
    public class MedicationDataStore
    {
        [Key]
        public int MedicineStoreId { get; set; }


        [ForeignKey("OrderMedications")]
        public int OrderMedicationID { get; set; }
        public virtual OrderMedications GetOrder { get; set; }

        [ForeignKey("MedicationRecords")]
        public int MedicationID { get; set; }
        public virtual MedicationRecords Medication { get; set; }

        public int Quantity { get; set; }

        public string Note { get; set; }
    }
}
