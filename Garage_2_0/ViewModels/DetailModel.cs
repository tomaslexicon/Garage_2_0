using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Garage_2_0.Models;

namespace Garage_2_0.ViewModels
{
    public class DetailModel
    {

        public int Id { get; set; }
        public VehicleType Type { get; set; }
        public string RegNo { get; set; }
        public string Color { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public int NumberOfWheels { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime ParkingTime { get; set; }


    }
}