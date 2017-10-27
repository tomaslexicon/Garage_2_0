using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Garage_2_0.ViewModels
{
    public class CheckOutModel
    {
        public int Id { get; set; }

        [Display(Name = "Registration number")]
        public string RegNo { get; set; }

        [Display(Name = "Start time")]
        public string StartTime { get; set; }
    }
}