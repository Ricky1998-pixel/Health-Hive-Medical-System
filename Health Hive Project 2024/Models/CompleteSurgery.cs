namespace Health_Hive_Project_2024.Models
{
    public class CompleteSurgery
    {
        public int CompleteSurgeryID { get; set; }
        public int SurgeryID { get; set; }
        public string? SurgeonID { get; set; } // Nullable to match SurgeryBooking
        public int? PatientID { get; set; } // Nullable to match SurgeryBooking
        public string? AnaesthesiologistID { get; set; } // Nullable to match SurgeryBooking
        public int? TheatreID { get; set; } // Nullable to match SurgeryBooking
        public DateTime SurgeryDate { get; set; }
        public string Session { get; set; }




        // Navigation properties
        public MedicalProfessionalRecords Surgeon { get; set; } // Assuming MedicalProfessional class for Surgeon
        public Patient Patient { get; set; }
        public MedicalProfessionalRecords Anaesthesiologist { get; set; } // Assuming MedicalProfessional class for Anaesthesiologist
        public OperatingTheatreRecords Theatre { get; set; } // Assuming Theatre class exists

        // Navigation properties if needed
        public ICollection<SurgeryBookingTreatment> SurgeryBookingTreatments { get; set; }
    }

}
