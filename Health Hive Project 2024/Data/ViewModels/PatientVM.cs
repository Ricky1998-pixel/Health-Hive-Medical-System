using Health_Hive_Project_2024.Models;

namespace Health_Hive_Project_2024.Data.ViewModels
{
    public class PatientVM
    {
        public string PatientId { get; set; }
        public string Type { get; set; }
        public int Val { get; set; }
        public int Val2 { get; set; }
        public IEnumerable<PatientVitals> PatientVitals { get; set; }
    }
}
