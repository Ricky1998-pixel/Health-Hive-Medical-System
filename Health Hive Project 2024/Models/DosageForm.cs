using System.ComponentModel.DataAnnotations;

namespace Health_Hive_Project_2024.Models
{
    public class DosageForm
    {
        [Key]
       public int DosageFormID {get;set;}
       public string Form {get;set;}

    }
}
