using System.ComponentModel.DataAnnotations;

namespace Garage_2_0.ViewModels
{
    public class ReceiptModel
    {
        public int Id { get; set; }

        [Display(Name = "Registration number")]
        public string RegNo { get; set; }

        [Display(Name = "Start time")]
        public string StartTime { get; set; }

        [Display(Name = "Stop time")]
        public string StopTime { get; set; }

        [Display(Name = "Parking time")]
        public string ParkingTime { get; set; }

        [Display(Name = "Cost")]
        public string ParkingCost { get; set; }

        [Display(Name = "Last name")]
        public string LastName { get; set; }

        [Display(Name = "First name")]
        public string FirstName { get; set; }
    }
}