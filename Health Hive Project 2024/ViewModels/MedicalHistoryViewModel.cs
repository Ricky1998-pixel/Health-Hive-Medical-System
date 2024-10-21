using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Health_Hive_Project_2024.Models;

namespace Health_Hive_Project_2024.ViewModels
{
    public class MedicalHistoryViewModel
    {
        public int MedicalHistoryID { get; set; }

        [Required]
        public int PatientID { get; set; }
        public Patient Patient { get; set; }

        [Required]
        public DateTime DateRecorded { get; set; }

        [Required]
        public int ConditionID { get; set; }
        public Condition Condition { get; set; }
        public List<Condition> AvailableConditions { get; set; }

        public string Treatment { get; set; }
        public string Medications { get; set; }
        public string Notes { get; set; }

        [Required]
        public string RecordedBy { get; set; }

        // Additional properties
        public string FamilyHistory { get; set; }
        public string SurgicalHistory { get; set; }
        public string Immunizations { get; set; }

        // List of selected allergy IDs
        public List<int> SelectedAllergyIDs { get; set; } = new List<int>();

        // List of available allergies
        public List<Allergy> AvailableAllergies { get; set; } = new List<Allergy>();
    }
}
