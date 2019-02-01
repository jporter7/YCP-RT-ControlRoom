namespace ControlRoomApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Jan31 : DbMigration
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
                        telescope_id = c.Int(nullable: false),
                        status = c.String(nullable: false, unicode: false),
                    })
                .PrimaryKey(t => t.Id);
            
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
            DropTable("dbo.rf_data");
            DropTable("dbo.orientation");
            DropTable("dbo.coordinate");
            DropTable("dbo.appointment");
        }
    }
}
