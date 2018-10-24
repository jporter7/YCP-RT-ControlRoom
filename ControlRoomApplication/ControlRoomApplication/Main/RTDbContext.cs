using System.Data.Entity;
using ControlRoomApplication.Constants;
using ControlRoomApplication.Entities;

namespace ControlRoomApplication.Main
{
    public class RTDbContext : DbContext
    {
        public RTDbContext() : base(GenericConstants.LOCAL_DATABASE_NAME)
        {

        }

        public RTDbContext(string connectionString) : base(connectionString)
        {
            Database.Connection.ConnectionString = connectionString;
            Configuration.LazyLoadingEnabled = true;
        }

        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<RFData> RFDatas { get; set; }
    }
}