namespace ControlRoomApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RFDataMigration : DbMigration
    {
        public override void Up()
        {
            DropTable("dbo.rf_data");
            DropTable("dbo.orientation");

            CreateTable(
                "dbo.rf_data",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        time_captured = c.DateTime(nullable: false, precision: 0),
                        intensity = c.Long(nullable: false),
                        appointment_id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id);

            CreateTable(
                "dbo.orientation",
                c => new
                {
                    CapturedOrientationId = c.Int(nullable: false, identity: true),
                    azimuth = c.Long(nullable: false),
                    elevation = c.Long(nullable: false),
                })
                .PrimaryKey(t => t.CapturedOrientationId)
                .ForeignKey("dbo.rf_data", t => t.CapturedOrientationId);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.orientation", "CapturedOrientationId", "dbo.rf_data");
            DropIndex("dbo.orientation", new[] { "CapturedOrientationId" });
            DropTable("dbo.orientation");
            DropTable("dbo.rf_data");
        }
    }
}
