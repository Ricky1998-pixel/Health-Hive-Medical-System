using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Health_Hive_Project_2024.Models
{
    public class VitalModel
    {
        [Key]
        public int VitalID { get; set; }


        [Required(ErrorMessage = "Minimum Value is required")]
        [Display(Name = "Min Value")]
        public string ReadingValue { get; set; }


       


        [ForeignKey("NormalRangeId")]
        public int NormalRangeId { get; set; }
        public virtual NormalRange GetNormalRange { get; set; }


        [ForeignKey("Patient")]
        public int PatientID { get; set; }
        public virtual Patient Patient { get; set; }

        [Display(Name = "Date")]
        [DataType(DataType.DateTime)]
        public DateTime TimeStamp { get; set; } = DateTime.Now;

       
        public string Id { get; set; }
    
    }
}
