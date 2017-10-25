using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Garage_2_0.Models;
using System.ComponentModel.DataAnnotations;

namespace Garage_2_0.ViewModels
{
    public class OverviewVehicle
    {
        [Display(Name = "Type")]
        public VehicleType Type { get; set; }

        [Display(Name = "Registration Number")]
        public string RegNo { get; set; }

        [Display(Name = "Color")]
        public string Color { get; set; }

        [Display(Name = "Start Time")]
        public DateTime StartTime { get; set; }
        public int Id { get; set; }
    }

    public class OverviewModel
    {
        public List<OverviewVehicle> Vehicles { get; set; }
        public bool IsDescending { get; set; }
        public string SortBy { get; set; }
        public string Search { get; set; }
    }
}