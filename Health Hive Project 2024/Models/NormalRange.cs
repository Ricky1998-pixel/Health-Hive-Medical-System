using System.ComponentModel.DataAnnotations;

namespace Health_Hive_Project_2024.Models
{
    public class NormalRange
    {
        [Key]
        public int NormalRangeId { get; set; }


        [Required(ErrorMessage = "Vital Name is required")]
        [Display(Name = "Name")]
        public string VitalName { get; set; }

        [Required(ErrorMessage = "Minimum Value is required")]
        [Display(Name = "Min Value")]
        public int MinValue { get; set; }

        [Required(ErrorMessage = "Maximum Value is required")]
        [Display(Name = "Max Value")]
        public int MaxValue { get; set; }
    }
}
