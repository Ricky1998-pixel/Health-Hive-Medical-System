using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Health_Hive_Project_2024.Models
{
    public class NortificationModel
    {
        [Key]
        public int NotifyId { get; set; }


        [ForeignKey("Anae_Id")]
        public string Id { get; set; }
        public virtual MedicalProfessionalRecords GetAnaesthesiologist { get; set; }

        public string Description { get; set; }

        public DateTime TimeStamp { get; set; } = DateTime.Now;
    }
}
