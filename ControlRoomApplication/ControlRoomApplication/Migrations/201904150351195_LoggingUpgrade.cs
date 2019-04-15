namespace ControlRoomApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LoggingUpgrade : DbMigration
    {
        public override void Up()
        {
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
            DropTable("dbo.telescope_log");
        }
    }
}
