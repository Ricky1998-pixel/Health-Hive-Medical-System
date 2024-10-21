using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Health_Hive_Project_2024.Models
{
    public class BedRecords
    {
        [Key]
        public int BedID { get; set; }
        [Required]
        [Display(Name = "Bed Number")]
        public string BedNo { get; set; }
        public bool IsAvailable { get; set; } = true;

        //[ForeignKey("Ward")]
        //[Display(Name = "Ward")]
        //public int WardID { get; set; }
        //public virtual WardRecords Ward { get; set; }
    }
}
