using System.ComponentModel.DataAnnotations;

namespace Health_Hive_Project_2024.Models
{
    public class Vitals
    {
        [Key]
        public int VitalID { get; set; }

        [Required(ErrorMessage = "vital Name is required")]
        [Display(Name = "Name")]
        public string Name { get; set; }

        
        [Display(Name = "Vital Name")]
        public string? VitalName { get; set; } // if vital has one value make 'VitalName' the same as 'Name'

      
        [Display(Name = "Vital Name")]
        public string? VitalName2 { get; set; }


        [Required(ErrorMessage = "Minimum Value is required")]
        [Display(Name = "Min Value")]
        public int MinimumValue { get; set; }

        [Required(ErrorMessage = "Maximum Value is required")]
        [Display(Name = "Max Value")]
        public int MaxValue { get; set; }

        
        public string UnitOfMeasure { get; set; }

    }
}
