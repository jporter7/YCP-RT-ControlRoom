using System.Data.Entity;
using ControlRoomApplication.Entities;

namespace ControlRoomApplication.Main
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
