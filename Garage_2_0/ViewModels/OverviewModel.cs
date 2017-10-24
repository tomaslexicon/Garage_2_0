using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Garage_2_0.Models;

namespace Garage_2_0.ViewModels
{
    public class OverviewVehicle
    {
        public VehicleType Type { get; set; }
        public string RegNo { get; set; }
        public string Color { get; set; }
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