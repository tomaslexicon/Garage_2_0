using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace Garage_2_0.DataAccessLayer
{
    public class GarageContext : DbContext
    {
        public DbSet<Models.ParkedVehicle> ParkedVehicles { get; set; }
        public DbSet<Models.Member> Members { get; set; }
        public DbSet<Models.VehicleType> VehicleTypes { get; set; }

        public GarageContext() : base("Garage25Connection")
        {
            
        }
    }
}