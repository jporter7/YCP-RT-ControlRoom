namespace ControlRoomApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Milestone3_Migration : DbMigration
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
                        coordinate_id = c.Int(nullable: false),
                        status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.rf_data",
                c => new
                {
                    id = c.Int(nullable: false, identity: true),
                    time_captured = c.DateTime(nullable: false, precision: 0),
                    intensity = c.Long(nullable: false),
                    appointment_id = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.appointment", t => t.appointment_id, cascadeDelete: true);
                //.Index(t => t.appointment_id);
            
            CreateTable(
                "dbo.coordinate",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        right_ascension = c.Double(nullable: false),
                        declination = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.orientation",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        azimuth = c.Double(nullable: false),
                        elevation = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.rf_data", "appointment_id", "dbo.appointment");
            DropIndex("dbo.rf_data", new[] { "appointment_id" });
            DropTable("dbo.orientation");
            DropTable("dbo.coordinate");
            DropTable("dbo.rf_data");
            DropTable("dbo.appointment");
        }
    }
}
