using Health_Hive_Project_2024.Models;

namespace Health_Hive_Project_2024.ViewModels
{
    public class PatientDetailsViewModel
    {
        public Patient Patient { get; set; }
        public List<string> Alerts { get; set; }
    }
}
