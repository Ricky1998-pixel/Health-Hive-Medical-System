using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Health_Hive_Project_2024.Models
{
    public class OrderItem
    {
        [Key]
        public int ItemID { get; set; }

        [Required]
        [ForeignKey("Medication")]
        public int MedicationID { get; set; }
        public virtual MedicationRecords Medication { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than zero.")]
        public int Quantity { get; set; }

        // New property to indicate if the order item is urgent
        public bool IsUrgent { get; set; }
    }
}
