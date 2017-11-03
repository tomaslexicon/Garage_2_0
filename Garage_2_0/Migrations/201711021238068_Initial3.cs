namespace Garage_2_0.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial3 : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.ParkedVehicles", "MemberId");
            CreateIndex("dbo.ParkedVehicles", "VehicleTypeId");
            AddForeignKey("dbo.ParkedVehicles", "MemberId", "dbo.Members", "Id", cascadeDelete: true);
            AddForeignKey("dbo.ParkedVehicles", "VehicleTypeId", "dbo.VehicleTypes", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ParkedVehicles", "VehicleTypeId", "dbo.VehicleTypes");
            DropForeignKey("dbo.ParkedVehicles", "MemberId", "dbo.Members");
            DropIndex("dbo.ParkedVehicles", new[] { "VehicleTypeId" });
            DropIndex("dbo.ParkedVehicles", new[] { "MemberId" });
        }
    }
}
