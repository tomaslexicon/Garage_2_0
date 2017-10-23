namespace Garage_2_0.Migrations
{
    using Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Garage_2_0.DataAccessLayer.GarageContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Garage_2_0.DataAccessLayer.GarageContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            context.ParkedVehicles.AddOrUpdate(
              p => p.RegNo,
              new ParkedVehicle
              {
                Type = VehicleType.Car,
                RegNo = "ABC123",
                Color = "Blue",
                Brand = "Volvo",
                Model = "240",
                NumberOfWheels = 4,
                StartTime = DateTime.Now
              }

            );

        }
    }
}
