using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Garage_2_0.Models;

namespace Garage_2_0.ViewModels
{
    public class StatisticsModel
    {

        [Display(Name = "Number of vehicles")]
        public int NumberOfVehicles { get; set; }

        [Display(Name = "Most popular brand")]
        public string mostPopularBrand { get; set; }

        [Display(Name = "Total number of wheels")]
        public int TotalNumberOfWheels { get; set; }

        [Display(Name = "Total cost all vehicles")]
        public int TotalCost { get; set; }

    }
}