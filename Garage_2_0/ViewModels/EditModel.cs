﻿using Garage_2_0.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Garage_2_0.ViewModels
{
    public class EditModel
    {
        public int Id { get; set; }

        [Display(Name = "Types")]
        public IEnumerable<SelectListItem> VehicleTypes { get; set; }

        [Display(Name = "Type")]
        public int Type { get; set; }

        [Display(Name = "Member")]
        public IEnumerable<SelectListItem> Members { get; set; }

        [Display(Name = "Member Id")]
        public int MemberId { get; set; }

        [Required]
        [Display(Name = "Registration number")]
        [StringLength(1024, ErrorMessage = "{0} needs to be at least {2} characters long", MinimumLength = 1)]
        public string RegNo { get; set; }

        public string OriginalRegNo { get; set; }

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

        [Display(Name = "Start time")]
        public DateTime StartTime { get; set; }
    }
}