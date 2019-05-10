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
            Configuration.LazyLoadingEnabled = true;
        }

        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
        //    //base.OnModelCreating(modelBuilder);
        //    //modelBuilder.Entity<appointment>()
        //    //  .HasOptional<orientation>(o => o.orientation);
        //    //modelBuilder.Entity<appointment>()
        //      //  .HasRequired<celestial_body>(c => c.celestial_body)
        //        //.WithMany(a => a.appointment)
        //        //.HasForeignKey<int>(c => c.celestial_body_id);
        //}

        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<RFData> RFDatas { get; set; }
        public DbSet<Orientation> Orientations { get; set; }
        public DbSet<Coordinate> Coordinates { get; set; }
        public DbSet<Log> Logs { get; set; }
    }
}