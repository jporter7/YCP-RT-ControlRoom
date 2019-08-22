using System.Data.Entity;
using ControlRoomApplication.Constants;
using ControlRoomApplication.Entities;

namespace ControlRoomApplication.Main
{
    public class RTDbContext : DbContext
    {
        public RTDbContext() : base(MiscellaneousConstants.LOCAL_DATABASE_NAME)
        {

        }

        public RTDbContext(string connectionString) : base(connectionString)
        {
            Database.Connection.ConnectionString = connectionString;
            Configuration.LazyLoadingEnabled = false;
        }

        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<RFData> RFDatas { get; set; }
        public DbSet<Orientation> Orientations { get; set; }
        public DbSet<Coordinate> Coordinates { get; set; }
        public DbSet<Log> Logs { get; set; }
        public DbSet<Temperature> Temperatures { get; set; }
        public DbSet<Acceleration> Accelerations { get; set; }
        

    }
}