
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Health_Hive_Project_2024.Models
{
    public class PatientVitals
    {
        [Key]
        public int PatientVitalID { get; set; }


        [ForeignKey("Patient")]
        [Display(Name = "Patient")]
        public int PatientID { get; set; }
        public virtual Patient? Patient { get; set; }
        [ForeignKey("Vitals")]

        public int VitalID { get; set; }
        public virtual Vitals? Vitals { get; set; }

        public int Value1 { get; set; }
        public int? Value2 { get; set; }
        public int? Value3 { get; set; }

        public DateTime DateTime {  get; set; } = DateTime.Now;

        public string? Notes { get; set; } // Additional notes if any
    }
}