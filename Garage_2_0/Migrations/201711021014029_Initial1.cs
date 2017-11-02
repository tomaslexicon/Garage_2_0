namespace Garage_2_0.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Members",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LastName = c.String(),
                        FirstName = c.String(),
                        MembershipId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.VehicleTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Type = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.ParkedVehicles", "MemberId", c => c.String());
            AddColumn("dbo.ParkedVehicles", "VehicleTypeId", c => c.Int(nullable: false));
            DropColumn("dbo.ParkedVehicles", "Type");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ParkedVehicles", "Type", c => c.Int(nullable: false));
            DropColumn("dbo.ParkedVehicles", "VehicleTypeId");
            DropColumn("dbo.ParkedVehicles", "MemberId");
            DropTable("dbo.VehicleTypes");
            DropTable("dbo.Members");
        }
    }
}
