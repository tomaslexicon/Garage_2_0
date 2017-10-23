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
              },
              new ParkedVehicle
              {
                  Type = VehicleType.Motorcycle,
                  RegNo = "XYZ123",
                  Color = "Red",
                  Brand = "Honda",
                  Model = "220SX",
                  NumberOfWheels = 2,
                  StartTime = DateTime.Now
              },
              new ParkedVehicle
              {
                  Type = VehicleType.Car,
                  RegNo = "QWE345",
                  Color = "Blue",
                  Brand = "BMW",
                  Model = "530i",
                  NumberOfWheels = 4,
                  StartTime = DateTime.Now
              },
              new ParkedVehicle
              {
                  Type = VehicleType.Bus,
                  RegNo = "RYU567",
                  Color = "Yellow",
                  Brand = "Scania",
                  Model = "550TX",
                  NumberOfWheels = 6,
                  StartTime = DateTime.Now
              },
              new ParkedVehicle
              {
                  Type = VehicleType.Boat,
                  RegNo = "FY17",
                  Color = "Black",
                  Brand = "Fjord",
                  Model = "997xT",
                  NumberOfWheels = 0,
                  StartTime = DateTime.Now
              },
              new ParkedVehicle
              {
                  Type = VehicleType.Car,
                  RegNo = "HMM123",
                  Color = "Green",
                  Brand = "Ferrari",
                  Model = "Testarossa",
                  NumberOfWheels = 4,
                  StartTime = DateTime.Now
              },
            new ParkedVehicle
            {
                Type = VehicleType.Car,
                RegNo = "ABC789",
                Color = "Pink",
                Brand = "SAAB",
                Model = "99",
                NumberOfWheels = 4,
                StartTime = DateTime.Now
            },
              new ParkedVehicle
              {
                  Type = VehicleType.Motorcycle,
                  RegNo = "IOT888",
                  Color = "White",
                  Brand = "Honda",
                  Model = "440SX",
                  NumberOfWheels = 2,
                  StartTime = DateTime.Now
              },
              new ParkedVehicle
              {
                  Type = VehicleType.Car,
                  RegNo = "QWE898",
                  Color = "Blue",
                  Brand = "Audi",
                  Model = "S77",
                  NumberOfWheels = 4,
                  StartTime = DateTime.Now
              }
            );

        }
    }
}
