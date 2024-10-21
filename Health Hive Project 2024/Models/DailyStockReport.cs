using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Humanizer;

namespace Health_Hive_Project_2024.Models
{
    public class DailyStockReport
    {
        [Key]
        public int ReportID { get; set; }

        [ForeignKey("Medication")]
        public int MedicationID { get; set; }
        public virtual MedicationRecords Medication { get; set; }

        public int QuantityUsed { get; set; }
        public DateTime DateUsed { get; set; }
    }

}


//Using this model and the StockManagement.cs
//In this setup:
//The StockManagement table tracks medication stock, including the quantity received, current stock levels, and re-order levels.
//The DailyStockReport table stores daily stock usage information, including the medication used, quantity used, and date of usage.

//You can then create methods or controllers in your application to handle stock management operations such as receiving stock, viewing stock levels, ordering stock, and generating daily stock usage reports based on these table structures. Adjust the data types and properties as needed to fit your specific requirements and business logic.
