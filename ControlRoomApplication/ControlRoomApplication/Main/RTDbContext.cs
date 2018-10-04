using System.Data.Entity;

namespace ControlRoomApplication
{
    public class RTDbContext : DbContext
    {
        public RTDbContext() : base()
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
    }
}
