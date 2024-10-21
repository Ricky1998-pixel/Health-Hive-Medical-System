using Humanizer;
using Microsoft.VisualBasic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Health_Hive_Project_2024.Models
{
    public class ConditionDiagnosisRecords
    {
        [Key]
        public int DiagnosisID { get; set; }

        [ForeignKey("Patient")]
        [Display(Name = "Patient")]
        public int PatientID { get; set; }
        public virtual Patient Patient { get; set; }

       [ForeignKey("ConditionId")]
        [Display(Name = "Condtion")]
        public int ConditionID { get; set; }
        public virtual Condition Condition { get; set; }

       
        

        
    }
}
