using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Health_Hive_Project_2024.Models
{
    public class WardRecords
    {
        [Key]
        public int WardID { get; set; }

        [Required(ErrorMessage = "Ward Name is required")]
        [Display(Name = "Ward Name")]
        public string WardName { get; set; }

        [Required(ErrorMessage = "Capacity is required")]
        [Display(Name = "Capacity")]
        public int Capacity { get; set; }

        [Required(ErrorMessage = "Location is required")]
        [Display(Name = "Location")]
        public string Location { get; set; }

        [Required(ErrorMessage = "Ward Type is required")]
        [Display(Name = "Ward Type")]
        public string WardType { get; set; }
    }
}
