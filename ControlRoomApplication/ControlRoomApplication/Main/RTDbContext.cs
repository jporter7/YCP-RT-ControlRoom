using System.Data.Entity;
using ControlRoomApplication.Entities;

namespace ControlRoomApplication.Main
{
    public class RTDbContext : DbContext
    {
        public RTDbContext() : base(AWSConstants.REMOTE_DATABASE_NAME)
        {

        }

        public RTDbContext(string connectionString) : base(connectionString)
        {
            Database.Connection.ConnectionString = connectionString;
            Configuration.LazyLoadingEnabled = true;
        }

        public DbSet<Appointment> Appointments { get; set; }
    }
}