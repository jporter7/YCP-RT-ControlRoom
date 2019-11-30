namespace ControlRoomApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LoggingUpdgrades : DbMigration
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
                        status = c.Int(nullable: false),
                        type = c.Int(nullable: false),
                        CelestialBody_Id = c.Int(),
                        Orientation_Id = c.Int(),
                        SpectraCyberConfig_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.celestial_body", t => t.CelestialBody_Id)
                .ForeignKey("dbo.orientation", t => t.Orientation_Id)
                .ForeignKey("dbo.spectracyber_config", t => t.SpectraCyberConfig_Id, cascadeDelete: true)
                .Index(t => t.CelestialBody_Id)
                .Index(t => t.Orientation_Id)
                .Index(t => t.SpectraCyberConfig_Id);
            
            CreateTable(
                "dbo.celestial_body",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        name = c.String(nullable: false, unicode: false),
                        Coordinate_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.coordinate", t => t.Coordinate_Id)
                .Index(t => t.Coordinate_Id);
            
            CreateTable(
                "dbo.coordinate",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        right_ascension = c.Double(nullable: false),
                        declination = c.Double(nullable: false),
                        Appointment_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.appointment", t => t.Appointment_Id)
                .Index(t => t.Appointment_Id);
            
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
                        appointment_id = c.Int(),
                        time_captured = c.DateTime(nullable: false, precision: 0),
                        intensity = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.appointment", t => t.appointment_id)
                .Index(t => t.appointment_id);
            
            CreateTable(
                "dbo.spectracyber_config",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        mode = c.Int(nullable: false),
                        integration_time = c.Int(nullable: false),
                        offset_voltage = c.Double(nullable: false),
                        if_gain = c.Double(nullable: false),
                        dc_gain = c.Int(nullable: false),
                        bandwidth = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.telescope_log",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        log_date = c.DateTime(nullable: false, precision: 0),
                        thread = c.String(nullable: false, unicode: false),
                        log_level = c.String(nullable: false, unicode: false),
                        logger = c.String(nullable: false, unicode: false),
                        message = c.String(nullable: false, unicode: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.appointment", "SpectraCyberConfig_Id", "dbo.spectracyber_config");
            DropForeignKey("dbo.rf_data", "appointment_id", "dbo.appointment");
            DropForeignKey("dbo.appointment", "Orientation_Id", "dbo.orientation");
            DropForeignKey("dbo.coordinate", "Appointment_Id", "dbo.appointment");
            DropForeignKey("dbo.appointment", "CelestialBody_Id", "dbo.celestial_body");
            DropForeignKey("dbo.celestial_body", "Coordinate_Id", "dbo.coordinate");
            DropIndex("dbo.rf_data", new[] { "appointment_id" });
            DropIndex("dbo.coordinate", new[] { "Appointment_Id" });
            DropIndex("dbo.celestial_body", new[] { "Coordinate_Id" });
            DropIndex("dbo.appointment", new[] { "SpectraCyberConfig_Id" });
            DropIndex("dbo.appointment", new[] { "Orientation_Id" });
            DropIndex("dbo.appointment", new[] { "CelestialBody_Id" });
            DropTable("dbo.telescope_log");
            DropTable("dbo.spectracyber_config");
            DropTable("dbo.rf_data");
            DropTable("dbo.orientation");
            DropTable("dbo.coordinate");
            DropTable("dbo.celestial_body");
            DropTable("dbo.appointment");
        }
    }
}
