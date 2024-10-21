using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Health_Hive_Project_2024.Models
{
    public class SurgeryBookingTreatment
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("SurgeryBooking")]
        public int SurgeryID { get; set; }
        public virtual SurgeryBooking SurgeryBooking { get; set; }

        [ForeignKey("TreatmentRecords")]
        public int TreatmentID { get; set; }
        public virtual TreatmentRecords TreatmentRecords { get; set; }


    }
}
