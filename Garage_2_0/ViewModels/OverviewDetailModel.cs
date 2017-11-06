using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Garage_2_0.Models;
using System.ComponentModel.DataAnnotations;

namespace Garage_2_0.ViewModels
{
    public class OverviewDetailVehicle
    {
        [Display(Name = "Type")]
        public string Type { get; set; }

        [Display(Name = "Registration number")]
        public string RegNo { get; set; }

        [Display(Name = "Owner name")]
        public string OwnerName { get; set; }

        [Display(Name = "Brand")]
        public string Brand { get; set; }

        [Display(Name = "Color")]
        public string Color { get; set; }

        [Display(Name = "Number of wheels")]
        public int NumberOfWheels { get; set; }

        [Display(Name = "Start time")]
        public DateTime StartTime { get; set; }

        [Display(Name = "Last name")]
        public string LastName { get; set; }

        [Display(Name = "First name")]
        public string FirstName { get; set; }

        [Display(Name = "Member number")]
        public int MembershipId { get; set; }

        public int Id { get; set; }
    }


    public class OverviewDetailModel
    {
        public List<OverviewDetailVehicle> Vehicles { get; set; }
        public bool IsDescending { get; set; }
        public string SortBy { get; set; }
        public string Search { get; set; }

    }
}