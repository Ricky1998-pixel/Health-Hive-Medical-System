using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;


namespace Health_Hive_Project_2024.Models
{
    public class OrderMedication
    {

        [Key]
        public int OrderMedicationID { get; set; }

        [Required]
        [StringLength(100)]
        public string AnaesthesiologistName { get; set; }

        [Required]
        [StringLength(100)]
        public string PatientName { get; set; }

        [Required]
        public DateTime OrderDate { get; set; }

        [Required]
        [StringLength(50)]
        public string OrderStatus { get; set; } = "Pending";

        public bool OrderUrgency { get; set; } = false;

        // Navigation property for related MedicationOrders
        public virtual ICollection<MedicationOrder> MedicationOrders { get; set; } = new List<MedicationOrder>();
    }

    public class MedicationOrder
    {
        [Key]
        public int MedicationOrderID { get; set; }

        [Required]
        [StringLength(100)]
        public string MedicationName { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
        public int Quantity { get; set; }

        // Foreign key to OrderMedication
        [ForeignKey("OrderMedication")]
        public int OrderMedicationID { get; set; }

        // Navigation property
        public virtual OrderMedication OrderMedication { get; set; }
    }
}
