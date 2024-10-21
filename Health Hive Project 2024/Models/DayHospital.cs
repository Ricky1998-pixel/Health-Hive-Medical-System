using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Health_Hive_Project_2024.Models
{
    public class DayHospital
    {
        [Key]
        public int HospitalID { get; set; }

        [Required(ErrorMessage = "Hospital Name is required")]
        [Display(Name = "Hospital Name")]
        public string HospitalName { get; set; }

        [Required(ErrorMessage = "Address is required")]
        [Display(Name = "Address")]
        public string Address { get; set; }

        //[Required(ErrorMessage = "Postal Code is required")]
        [Display(Name = "Postal Code")]
        public string PostalCode { get; set; }

        //[Required(ErrorMessage = "Suburb is required")]
        [Display(Name = "Suburb")]
        public string Suburb { get; set; }

        //[Required(ErrorMessage = "City is required")]
        [Display(Name = "City")]
        public string City { get; set; }

        //[Required(ErrorMessage = "Province is required")]
        [Display(Name = "Province")]
        public string Province { get; set; }

        [Required(ErrorMessage = "Hospital Contact Number is required")]
        [Display(Name = "Hospital Contact Number")]
        public string HospitalContactNumber { get; set; }

        [Required(ErrorMessage = "Hospital Email Address is required")]
        [EmailAddress]
        [Display(Name = "Hospital Email Address")]
        public string HospitalEmaiAddress { get; set; }

        public string PracticeManage { get; set; }

        [Required(ErrorMessage = "Purchase Manager Email Address is required")]
        [EmailAddress]
        [Display(Name = "Purchase Manager Email Address")]
        public string PurchaseManagerEmailAddress { get; set; }
    }
}
