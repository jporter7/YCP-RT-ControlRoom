namespace ControlRoomApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Feb12 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.appointment",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        user_id = c.Int(nullable: false),
                        start_time = c.DateTime(nullable: false, precision: 0),
                        end_time = c.DateTime(nullable: false, precision: 0),
                        telescope_id = c.Int(nullable: false),
                        status = c.String(nullable: false, unicode: false),
                        type = c.String(nullable: false, unicode: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CelestialBodies",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AppointmentId = c.Int(nullable: false),
                        name = c.String(nullable: false, unicode: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.appointment", t => t.AppointmentId, cascadeDelete: true)
                .Index(t => t.AppointmentId);
            
            CreateTable(
                "dbo.coordinate",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AppointmentId = c.Int(nullable: false),
                        right_ascension = c.Double(nullable: false),
                        declination = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.appointment", t => t.AppointmentId, cascadeDelete: true)
                .Index(t => t.AppointmentId);
            
            CreateTable(
                "dbo.orientation",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        azimuth = c.Double(nullable: false),
                        elevation = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.rf_data",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        time_captured = c.DateTime(nullable: false, precision: 0),
                        intensity = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.coordinate", "AppointmentId", "dbo.appointment");
            DropForeignKey("dbo.CelestialBodies", "AppointmentId", "dbo.appointment");
            DropIndex("dbo.coordinate", new[] { "AppointmentId" });
            DropIndex("dbo.CelestialBodies", new[] { "AppointmentId" });
            DropTable("dbo.rf_data");
            DropTable("dbo.orientation");
            DropTable("dbo.coordinate");
            DropTable("dbo.CelestialBodies");
            DropTable("dbo.appointment");
        }
    }
}
