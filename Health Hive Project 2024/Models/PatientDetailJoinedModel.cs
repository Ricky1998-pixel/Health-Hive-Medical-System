using Health_Hive_Project_2024.Data.ViewModels;

namespace Health_Hive_Project_2024.Models
{

    public class PatientDetailJoinedModel
    {

        public Patient Patient { get; set; }

        public int PatientID { get; set; }

        public MedicalHistory MedicalHistory { get; set; }

        public List<OrderMedications> MedicationOrders { get; set; }

        public List<MedicationDataStore> dataStores { get; set; }

        public List<MedicationRecords> MedicationRecords { get; set; }

        public List<PatientVitals> GetVitals { get; set; }

        public List<Vitals> GetVitalsList { get; set; }

        public List<Allergies> GetAllergies { get; set; }

        public List<Condition> GetCondition { get; set; }

        public List<PatientMedication> GetCurrentMed { get; set; }

        public List<PatientAdmission> GetAdmission { get; set; }
    }
}
