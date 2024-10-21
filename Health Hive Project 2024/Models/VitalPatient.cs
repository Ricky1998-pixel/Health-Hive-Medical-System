using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Health_Hive_Project_2024.Models
{
    public class VitalPatient
    {
   
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int VitalPatientId { get; set; }

        [Required]
        [StringLength(100)]
        public string PatientName { get; set; }

        public List<PatientV> Vitals { get; set; } = new List<PatientV>();
    }

    public class PatientV
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PatientVId { get; set; }

        [Required]
        [StringLength(100)]
        public string VitalName { get; set; }

        [Required]
        public float MinValue { get; set; }

        [Required]
        public float MaxValue { get; set; }

        [Required]
        public float CurrentValue { get; set; }

        [Required]
        public DateTime DateRecorded { get; set; }

        public int VitalPatientId { get; set; }
        [ForeignKey("VitalPatientId")]
        public VitalPatient VitalPatient { get; set; }
    }
}
