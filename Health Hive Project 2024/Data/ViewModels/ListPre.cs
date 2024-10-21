using System.ComponentModel.DataAnnotations;

namespace Health_Hive_Project_2024.Data.ViewModels
{
    public class ListPre
    {

        public int PresId { get; set; } 
        public int PatientId { get; set; }
        public int Qty { get; set; }
        public int QtyAd { get; set; }
        [Display(Name ="Name")]
        public string PatName { get; set; }
        [Display(Name = "Surname")]

        public string PatSurname { get; set; }
        [Display(Name = "ID Number")]

        public string PatIDNo { get; set; } 
        public string Ward { get; set; } 
        public string Bed { get; set; } 
        public string Status { get; set; } 
    }
}
