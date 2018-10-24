using System;
using ControlRoomApplication.Constants;
using ControlRoomApplication.Entities;

namespace ControlRoomApplication.Main
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Create an instance of the DbContext (it does all of the data accessing
            // and manipulation). Then, add the user to the user table. Lastly, save the 
            // changes that were automatically checked by dbContext.ChangeTracker
            try
            {
                RTDbContext dbContext = new RTDbContext(GenericConstants.LOCAL_CONNECTION_STRING);

                Console.WriteLine("Appointments table: \n");
                foreach(Appointment app in dbContext.Appointments)
                {
                    Console.WriteLine(string.Join(Environment.NewLine,
                        $"Appointment {app.Id} has a start time of {app.StartTime} and an end time of {app.EndTime}. ",
                        $"The appointment's id is: {app.Id}",
                        ""));
                }

                Console.WriteLine("Would you like to add some RFData? (y/n)");
                var input = Console.ReadLine();

                if (input.ToLower().Equals("y"))
                {
                    RFData rf = new RFData();
                    rf.TimeCaptured = DateTime.Now;

                    rf.Intensity = 4245345;
                    rf.AppointmentId = 0;

                    dbContext.RFDatas.Add(rf);
                    dbContext.SaveChanges();
                }

                Console.ReadKey(true);
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e.Message);
                Console.ReadKey(true);
            }
        }
    }
}
