using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Health_Hive_Project_2024.Models;

namespace Health_Hive_Project_2024.Models
{
    public class AnaesthesiaOrder
    {
        [Key]
        public int OrderID { get; set; }


        
        [Required(ErrorMessage = "Patient Name is required")]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "Date")]
        [Required(ErrorMessage = "Last Updated Date Required.")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }


        [Required(ErrorMessage = "Medication Required")]
        [Display(Name = "Medication")]
        public string MedicationtoOrder { get; set; }

        [Required(ErrorMessage = "Quantity Medication is required")]
        [Display(Name = "Quantity")]
        public int Quantity { get; set; }

        [Display(Name = "Notes")]
        public string Notes { get; set; }

        [Display(Name = "OrderStatus")]
        public string OrderStatus { get; set; }

        [Display(Name = "OrderUrgency")]
        public string OrderUrgency { get; set; }

        [Required(ErrorMessage = "Anesthesiologist Name is required")]
        [ForeignKey("Anaesthesiologist")]
        public string Id { get; set; }
        public virtual MedicalProfessionalRecords Anaesthesiologist { get; set; }

        [Required(ErrorMessage = "Patient Name is required")]
        [ForeignKey("Patient")]
        public int PatientId { get; set; }
        public virtual Patient? Patient { get; set; }



        // Other properties as needed

    }
}

//Implement Contra-Indication Checks:
//You can add logic to the method where the anaesthesiologist adds medication to the order. Within this method, perform checks for contra-indications based on patient diagnosis, allergies, and medication interactions. If any contra-indication is found, handle it appropriately (e.g., display a warning message, prevent the medication from being added to the order, etc.).

//public void AddMedicationToOrder(int patientID, int anaesthesiologistID, int medicationID, int quantity)
//{
//    // Check for contra-indications
//    if (HasContraIndications(patientID, medicationID))
//    {
//        // Handle contra-indications (e.g., display warning message)
//        Console.WriteLine("Warning: Contra-indication found for this medication.");
//        return;
//    }

//    // Add medication to the order
//    var order = new AnaesthesiaOrder
//    {
//        PatientID = patientID,
//        AnaesthesiologistID = anaesthesiologistID,
//        MedicationID = medicationID,
//        Quantity = quantity
//    };

//    // Save order to database or perform further processing
//}

//public bool HasContraIndications(int patientID, int medicationID)
//{
//    // Logic to check for contra-indications based on patient and medication
//    // Return true if contra-indications are found, false otherwise
//    return false;
//}

