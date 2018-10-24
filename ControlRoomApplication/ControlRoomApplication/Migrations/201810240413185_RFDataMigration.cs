namespace ControlRoomApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RFDataMigration : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Appointments", newName: "appointment");
            CreateTable(
                "dbo.rf_data",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        time_captured = c.DateTime(nullable: false, precision: 0),
                        intensity = c.Long(nullable: false),
                        appointment_id = c.Int(nullable: false),
                        AcquisitionOrientation_Id = c.Int(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Orientations", t => t.AcquisitionOrientation_Id)
                .Index(t => t.AcquisitionOrientation_Id);
            
            CreateTable(
                "dbo.Orientations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        azimuth = c.Long(nullable: false),
                        elevation = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.rf_data", "AcquisitionOrientation_Id", "dbo.Orientations");
            DropIndex("dbo.rf_data", new[] { "AcquisitionOrientation_Id" });
            DropTable("dbo.Orientations");
            DropTable("dbo.rf_data");
            RenameTable(name: "dbo.appointment", newName: "Appointments");
        }
    }
}
