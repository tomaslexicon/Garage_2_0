namespace Garage_2_0.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ParkedVehicles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Type = c.Int(nullable: false),
                        RegNo = c.String(nullable: false, maxLength: 1024),
                        Color = c.String(nullable: false, maxLength: 1024),
                        Brand = c.String(nullable: false, maxLength: 1024),
                        Model = c.String(nullable: false, maxLength: 1024),
                        NumberOfWheels = c.Int(nullable: false),
                        StartTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ParkedVehicles");
        }
    }
}
