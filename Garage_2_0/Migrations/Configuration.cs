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


            VehicleType[] vehicleTypes = new[] {
                    new VehicleType { Type = "Car" },
                    new VehicleType { Type = "Motorcycle" },
                    new VehicleType { Type = "Bus" },
                    new VehicleType { Type = "Boat" },
                    new VehicleType { Type = "Airplane" }
                };

            context.VehicleTypes.AddOrUpdate(s => new { s.Type}, vehicleTypes);
            context.SaveChanges();


            Member[] members = new[] {
                    new Member { MembershipId = 1, FirstName = "Adam", LastName = "Olsson" },
                    new Member { MembershipId = 2, FirstName = "Bertil", LastName = "Persson" },
                    new Member { MembershipId = 3, FirstName = "Östen", LastName = "Ringdahl" },
                    new Member { MembershipId = 4, FirstName = "Niklas", LastName = "Ek" },
                    new Member { MembershipId = 5, FirstName = "Bo", LastName = "Östberg" },
                    new Member { MembershipId = 6, FirstName = "Jan", LastName = "Bååth" }
                };

            context.Members.AddOrUpdate(s => new { s.MembershipId, s.FirstName, s.LastName }, members);
            context.SaveChanges();


            context.ParkedVehicles.AddOrUpdate(
              p => p.RegNo,
              new ParkedVehicle
              {
                  VehicleTypeId = vehicleTypes[0].Id,
                  MemberId = members[0].Id,
                  RegNo = "ABC123",
                  Color = "Blue",
                  Brand = "Volvo",
                  Model = "240",
                  NumberOfWheels = 4,
                  StartTime = DateTime.Now
              },
              new ParkedVehicle
              {
                  VehicleTypeId = vehicleTypes[1].Id,
                  MemberId = members[1].Id,
                  RegNo = "XYZ123",
                  Color = "Red",
                  Brand = "Honda",
                  Model = "220SX",
                  NumberOfWheels = 2,
                  StartTime = DateTime.Now
              },
              new ParkedVehicle
              {
                  VehicleTypeId = vehicleTypes[0].Id,
                  MemberId = members[2].Id,
                  RegNo = "QWE345",
                  Color = "Blue",
                  Brand = "BMW",
                  Model = "530i",
                  NumberOfWheels = 4,
                  StartTime = DateTime.Now
              },
              new ParkedVehicle
              {
                  VehicleTypeId = vehicleTypes[2].Id,
                  MemberId = members[3].Id,
                  RegNo = "RYU567",
                  Color = "Yellow",
                  Brand = "Scania",
                  Model = "550TX",
                  NumberOfWheels = 6,
                  StartTime = DateTime.Now
              },
              new ParkedVehicle
              {
                  VehicleTypeId = vehicleTypes[3].Id,
                  MemberId = members[4].Id,
                  RegNo = "FY17",
                  Color = "Black",
                  Brand = "Fjord",
                  Model = "997xT",
                  NumberOfWheels = 0,
                  StartTime = DateTime.Now
              },
              new ParkedVehicle
              {
                  VehicleTypeId = vehicleTypes[0].Id,
                  MemberId = members[2].Id,
                  RegNo = "HMM123",
                  Color = "Green",
                  Brand = "Ferrari",
                  Model = "Testarossa",
                  NumberOfWheels = 4,
                  StartTime = DateTime.Now
              },
            new ParkedVehicle
            {
                VehicleTypeId = vehicleTypes[0].Id,
                MemberId = members[0].Id,
                RegNo = "ABC789",
                Color = "Pink",
                Brand = "SAAB",
                Model = "99",
                NumberOfWheels = 4,
                StartTime = DateTime.Now
            },
              new ParkedVehicle
              {
                  VehicleTypeId = vehicleTypes[1].Id,
                  MemberId = members[0].Id,
                  RegNo = "IOT888",
                  Color = "White",
                  Brand = "Honda",
                  Model = "440SX",
                  NumberOfWheels = 2,
                  StartTime = DateTime.Now
              },
              new ParkedVehicle
              {
                  VehicleTypeId = vehicleTypes[0].Id,
                  MemberId = members[2].Id,
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
