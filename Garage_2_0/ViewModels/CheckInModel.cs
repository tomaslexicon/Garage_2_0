using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Garage_2_0.Models;
using System.ComponentModel.DataAnnotations;

namespace Garage_2_0.ViewModels
{
    public class CheckInModel
    {
        public int Id { get; set; }

        [Display(Name = "Type")]
        public VehicleTypeEnum Type { get; set; }

        [Required]
        [Display(Name = "Registration number")]
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

        [Display(Name = "Number of wheels")]
        [Range(0, int.MaxValue, ErrorMessage = "{0} needs to be larger or equal to {2}")]
        public int NumberOfWheels { get; set; }
    }
}