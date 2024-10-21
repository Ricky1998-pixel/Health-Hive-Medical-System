using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Health_Hive_Project_2024.Models
{
    public class OperatingTheatreRecords
    {
        [Key]
        public int TheatreID { get; set; }

        [Required(ErrorMessage = "Theatre Name is required")]
        [Display(Name = "Theatre Name")]
        public string TheatreName { get; set; }

        
    }
}
