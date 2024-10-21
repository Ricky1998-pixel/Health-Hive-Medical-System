using Microsoft.AspNetCore.Mvc.Rendering;

namespace Health_Hive_Project_2024.Models
{
    public class ReportViewModel
    {
        public DateTime StartDate { get; set; } = DateTime.Today;  // Default to today
        public DateTime EndDate { get; set; } = DateTime.Today;    // Default to today
        public List<OrderMedications> Orders { get; set; }
        public string LoggedInUserFullName { get; set; }
        // Add a property for selected status
        public OrderStatus? SelectedStatus { get; set; }

        // Add a list of possible statuses for dropdown
        public List<SelectListItem> Statuses { get; set; }
    }
}
