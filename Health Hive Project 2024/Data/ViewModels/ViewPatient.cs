using Health_Hive_Project_2024.Models;

namespace Health_Hive_Project_2024.Data.ViewModels
{
    public class ViewPatient
    {
        public Patient Patient { get; set; }

        public IEnumerable<PatientVitals> Vitals { get; set; }
        public IEnumerable<PatientCondition> Condition { get; set; }
        public IEnumerable<PatientMedication> PatientMedication { get; set; }
        public IEnumerable<Allergies> Allergies { get; set; }
        public IEnumerable<MedicalHistory> MedicalHistory { get; set; }
    }
}
