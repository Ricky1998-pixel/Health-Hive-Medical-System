using System.ComponentModel.DataAnnotations;

namespace Health_Hive_Project_2024.Models
{
    public class VitalType // this is null please delete we using Vitals
    {
        [Key]
        public int VitalTypeID { get; set; }
        public string Type { get; set; }
        public int Value1 { get; set; }
        public int? Value2 { get; set; }
        public int? Value3 { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;

    }
}
