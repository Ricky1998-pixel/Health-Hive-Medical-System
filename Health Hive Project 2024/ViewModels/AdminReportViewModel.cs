using Health_Hive_Project_2024.Models;
using System.Collections.Generic;

namespace Health_Hive_Project_2024.ViewModels
{
    public class AdminReportViewModel
    {
        public DayHospital HospitalDetails { get; set; } = new DayHospital(); // Ensure this is initialized
        public int BedsCount { get; set; } = 0;
        public int MedicationsCount { get; set; } = 0;
        public int UsersCount { get; set; } = 0;
        public int TheatresCount { get; set; } = 0;
        public int ActiveIngredientsCount { get; set; } = 0;
        public List<MedicationRecords> Medications { get; set; } = new List<MedicationRecords>();
        public List<MedicalProfessionalRecords> Users { get; set; } = new List<MedicalProfessionalRecords>();
        public List<OperatingTheatreRecords> Theatres { get; set; } = new List<OperatingTheatreRecords>();
        public List<BedRecords> Beds { get; set; } = new List<BedRecords>();

        public string GetHospitalName() => HospitalDetails?.HospitalName ?? "N/A";
        public string GetHospitalAddress() => HospitalDetails?.Address ?? "N/A";
        public string GetHospitalContactNumber() => HospitalDetails?.HospitalContactNumber ?? "N/A";
        public string GetHospitalEmail() => HospitalDetails?.HospitalEmaiAddress ?? "N/A";
    }
}





