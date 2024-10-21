using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Health_Hive_Project_2024.Models
{

    public class OrderMedications
    {

        [Key]
        public int OrderMedicationID { get; set; }

        [ForeignKey("Patient")]
        public int PatientID { get; set; }
        public virtual Patient Patient { get; set; }

        [ForeignKey("Id")]
        public string? Id { get; set; }
        public virtual MedicalProfessionalRecords GetAnaesthesiologist { get; set; }

        public string PrescriptionOrder { get; set; }

       
        public virtual ICollection<MedicationDataStore> GetMedicationData { get; set; }

        public DateTime Date { get; set; } = DateTime.Now;

        
        public bool IsReceived { get; set; }

        [DisplayName("Status")]
        public OrderStatus Status { get; set; }

    
        public string PharmacistID { get; set; }
      


        public bool IsUrgent { get; set; }



        public string Instructions { get; set; }

    }
    public enum OrderStatus
    {
        Ordered,
        Dispensed,
        Rejected,
        Received
    }
}
