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
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public int MembershipId { get; set; }

        public string FullName => (FirstName + " " + LastName).Trim();

        // navigational properties
        public virtual ICollection<ParkedVehicle> ParkedVehicles { get; set; }
    }
}