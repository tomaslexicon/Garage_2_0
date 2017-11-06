using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Garage_2_0.Models
{
    //public enum VehicleTypeEnum
    //{
    //    Car,
    //    Motorcycle,
    //    Bus,
    //    Boat,
    //    Airplane
    //}

    public class ParkedVehicle
    {
        public int Id { get; set; }

        // Foreign Keys
        public int MemberId { get; set; }
        public int VehicleTypeId { get; set; }

        [Required]
        [Display(Name = "Registration Number")]
        [StringLength(1024, ErrorMessage = "{0} needs to be at least {2} characters long", MinimumLength = 1)]
        public string RegNo { get; set; }

        [Required]
        [Display(Name = "Color")]
        [StringLength(1024, ErrorMessage = "{0} needs to be at least {2} characters long", MinimumLength = 1)]
        public string Color { get; set; }

        [Required]
        [Display(Name = "Brand")]
        [StringLength(1024, ErrorMessage = "{0} needs to be at least {2} characters long", MinimumLength = 1)]
        public string Brand { get; set; }

        [Required]
        [Display(Name = "Model")]
        [StringLength(1024, ErrorMessage = "{0} needs to be at least {2} characters long", MinimumLength = 1)]
        public string Model { get; set; }

        [Display(Name = "Number Of Wheels")]
        [Range(0, int.MaxValue, ErrorMessage = "{0} needs to be larger or equal to {2}")]
        public int NumberOfWheels { get; set; }

        [Display(Name = "Start Time")]
        public DateTime StartTime { get; set; }

        // navigational properties
        public virtual Member Member { get; set; }
        public virtual VehicleType VehicleType { get; set; }
    }
}