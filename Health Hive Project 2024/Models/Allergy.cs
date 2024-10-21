using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Health_Hive_Project_2024.Models
{
    public class Allergy
    {
        [Key]
        public int AllergyID { get; set; }

        [Required]
        public string Name { get; set; }

        public string Severity { get; set; }

        //// Navigation property for medical histories
        //public virtual ICollection<MedicalHistoryAllergy> MedicalHistoryAllergies { get; set; } = new List<MedicalHistoryAllergy>();
    }
}




