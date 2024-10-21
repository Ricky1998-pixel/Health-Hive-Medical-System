using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Health_Hive_Project_2024.Models
{
    public class PatientAdmission
    {
        [Key]
        public int PatientAdmissionID { get; set; }

        [ForeignKey("Patient")]
        [Display(Name = "Patient")]
        public int PatientID { get; set; }
        public virtual Patient Patient { get; set; }

        public DateTime AdmissionDate { get; set; } = DateTime.Now;
        public DateTime? DischargeDate { get; set; }

        [ForeignKey("Nurse")]
        [Display(Name = "Nurse")]
        public string Id { get; set; } // changed nurseID to ID
        public virtual MedicalProfessionalRecords Nurse { get; set; }



        [ForeignKey("Ward")]
        [Display(Name = "Ward")]
        public int WardID { get; set; }
        public virtual WardRecords Ward { get; set; }

        [ForeignKey("Bed")]
        [Display(Name = "Bed")]
        public int BedID { get; set; }
        public virtual BedRecords Bed { get; set; }


        public int Height { get; set; }
        [ForeignKey(nameof(SurgeryBooking))]
        public int SurgeryBID { get; set; }
        public virtual SurgeryBooking? SurgeryBooking { get; set; }

        public int Weight { get; set; }
        public double BMI { get; set; }
    }
}

