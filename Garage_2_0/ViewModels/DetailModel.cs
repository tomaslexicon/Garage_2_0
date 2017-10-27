using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Garage_2_0.Models;
using System.ComponentModel.DataAnnotations;

namespace Garage_2_0.ViewModels
{
    public class DetailModel
    {

        public int Id { get; set; }

        [Display(Name = "Type")]
        public VehicleType Type { get; set; }
        [Display(Name = "Registration number")]
        public string RegNo { get; set; }
        [Display(Name = "Color")]
        public string Color { get; set; }
        [Display(Name = "Brand")]
        public string Brand { get; set; }
        [Display(Name = "Model")]
        public string Model { get; set; }
        [Display(Name = "Number of wheels")]
        public int NumberOfWheels { get; set; }
        [Display(Name = "Start time")]
        public string StartTime { get; set; }
        [Display(Name = "Parking time")]
        public string ParkingTime { get; set; }


    }
}