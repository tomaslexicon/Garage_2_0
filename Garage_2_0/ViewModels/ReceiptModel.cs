﻿using System.ComponentModel.DataAnnotations;

namespace Garage_2_0.ViewModels
{
    public class ReceiptModel
    {
        public int Id { get; set; }

        [Display(Name = "Registration Number")]
        public string RegNo { get; set; }

        [Display(Name = "Start Time")]
        public string StartTime { get; set; }

        [Display(Name = "Stop Time")]
        public string StopTime { get; set; }

        [Display(Name = "Parking Time")]
        public string ParkingTime { get; set; }

        [Display(Name = "Cost")]
        public string ParkingCost { get; set; }
    }
}