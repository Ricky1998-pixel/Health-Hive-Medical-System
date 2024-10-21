//using System.ComponentModel.DataAnnotations.Schema;
//using System.ComponentModel.DataAnnotations;

//namespace Health_Hive_Project_2024.Models
//{
//    public class PatientCondition
//    {
//        [Key]
//        public int PatientConditionID { get; set; }

//        [ForeignKey("Patient")]
//        [Display(Name = "Patient")]
//        public int PatientID { get; set; }
//        public virtual Patient Patient { get; set; }

//        [ForeignKey("Condition")]
//        [Display(Name = "Condition")]
//        public int ConditionID { get; set; }
//        public virtual Condition Condition { get; set; }
//    }
//}

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Health_Hive_Project_2024.Models
{
    public class PatientCondition
    {
        [Key]
        public int PatientConditionID { get; set; }

        [ForeignKey("Patient")]
        [Display(Name = "Patient")]
        public int PatientID { get; set; }
        public virtual Patient Patient { get; set; }

        [ForeignKey("Condition")]
        [Display(Name = "Condition")]
        public int ConditionID { get; set; }
        public virtual Condition Condition { get; set; }
    }
}


