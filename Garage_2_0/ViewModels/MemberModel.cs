using Garage_2_0.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Garage_2_0.ViewModels
{
    // Index view
    public class MemberView
    {
        public int Id { get; set; }

        [Display(Name = "First name")]
        public string FirstName { get; set; }

        [Display(Name = "Last name")]
        public string LastName { get; set; }

        [Display(Name = "Member number")]
        public int MembershipId { get; set; }
    }

    public class MemberViewModel
    {
        public List<MemberView> Members { get; set; }
        public bool IsDescending { get; set; }
        public string SortBy { get; set; }
        public string Search { get; set; }
    }


    // Details view
    public class MemberVehicle
    {
        public int Id { get; set; }

        [Display(Name = "Registration Number")]
        public string RegNo { get; set; }

        [Display(Name = "Brand")]
        public string Brand { get; set; }

        [Display(Name = "Model")]
        public string Model { get; set; }
    }

    public class MemberDetailsModel
    {
        public int Id { get; set; }

        [Display(Name = "First name")]
        public string FirstName { get; set; }

        [Display(Name = "Last name")]
        public string LastName { get; set; }

        [Display(Name = "Member nr")]
        public int MembershipId { get; set; }

        public List<MemberVehicle> MemberParkedVehicles { get; set; }
    }
}