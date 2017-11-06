using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Garage_2_0.Models
{
    public class Member
    {
        public int Id { get; set; }

        [Display(Name = "Last name")]
        public string LastName { get; set; }

        [Display(Name = "First name")]
        public string FirstName { get; set; }

        [Display(Name = "Member nr")]
        public int MembershipId { get; set; }

        // navigational properties
        public virtual ICollection<ParkedVehicle> ParkedVehicles { get; set; }
    }
}