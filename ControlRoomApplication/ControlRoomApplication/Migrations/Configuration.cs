using System.Data.Entity.Migrations;

namespace ControlRoomApplication.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<ControlRoomApplication.Main.RTDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            SetSqlGenerator("MySql.Data.MySqlClient", new MySql.Data.EntityFramework.MySqlMigrationSqlGenerator());
        }

        protected override void Seed(ControlRoomApplication.Main.RTDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
        }
    }
}
