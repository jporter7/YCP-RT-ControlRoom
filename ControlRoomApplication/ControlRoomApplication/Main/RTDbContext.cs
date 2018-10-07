using System.Data.Entity;

namespace ControlRoomApplication
{
    public class RTDbContext : DbContext
    {
        public RTDbContext() : base("rtdatabase")
        {
            Configuration.LazyLoadingEnabled = true;
        }

        public DbSet<Appointment> Appointments { get; set; }
    }
}
